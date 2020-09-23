using System.Collections.Generic;
using System.Threading.Tasks;
using ar.AzureFunctions.Sales.Data;

namespace ar.AzureFunctions.Sales.Services
{
    public interface ISalesService
    {
        Task<IEnumerable<ICustomer>> GetCustomers(IDictionary<string,string> criteria);
    }
}