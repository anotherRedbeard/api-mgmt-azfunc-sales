using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ar.AzureFunctions.Sales.Data;
using ar.AzureFunctions.Sales.Services;
using Dapper;
using Microsoft.Azure.Services.AppAuthentication;

namespace ar.AzureFunctions.Sales.Services
{
    public class SalesService : ISalesService
    {
        private static string _connectionString = Environment.GetEnvironmentVariable("SalesConnectionString");
        public async Task<IEnumerable<ICustomer>> GetCustomers(IDictionary<string, string> criteria)
        {
            List<Customer> results = new List<Customer>();
            //build filter 
            var sql = @"select *, customer_id as CustomerId, first_name as FirstName, last_name as LastName, zip_code as ZipCode from sales.customers where 1=1";

            string customerId, customerName, skip, take;
            var parameters = new Dictionary<string,object>();
            criteria.TryGetValue("customerId",out customerId);
            criteria.TryGetValue("customerName",out customerName);
            criteria.TryGetValue("skip",out skip);
            criteria.TryGetValue("take",out take);

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                parameters.Add("customerName",customerName);
                sql += $" and concat(first_name,' ',last_name) like '%' + @customerName + '%'";
            }

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                parameters.Add("customerId",customerId);
                sql += $" and customer_id = @customerId";
            }

            if (!string.IsNullOrWhiteSpace(skip) && !string.IsNullOrWhiteSpace(take))
            {
                int intSkip, intTake;
                var skipParse = int.TryParse(skip,out intSkip);
                var takeParse = int.TryParse(take,out intTake);
                if(!skipParse)
                {
                    throw new ArgumentException("skip must evaluate to an integer.  Please try using skip again with an integer value.");
                }
                if (!takeParse)
                {
                    throw new ArgumentException("take must evaluate to an integer.  Please try using skip again with an integer value.");
                }
                sql +=$" order by customerId offset {intSkip} rows FETCH NEXT {intTake} rows only";
            }
            else if (skip != take) 
            {
                throw new ArgumentException("Both skip and take must be included in your query, or neither should be included.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                connection.AccessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/");
                //check for params
                if (parameters.Count() > 0)
                {
                    DynamicParameters dbParams = new DynamicParameters();
                    dbParams.AddDynamicParams(parameters);
                    results = connection.Query<Customer>(sql,dbParams).ToList();
                }
                else
                {
                    results = connection.Query<Customer>(sql).ToList();
                }
            }

            return results;
        }
    }
}