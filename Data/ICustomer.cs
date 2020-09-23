namespace ar.AzureFunctions.Sales.Data
{
    public interface ICustomer
    {
        int CustomerId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Street { get; set; }
        string City { get; set; }
        string State { get; set; }
        string ZipCode { get; set; }
    }
}