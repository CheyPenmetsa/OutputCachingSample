namespace BusinessLogic.Data
{
    public interface ICustomerRepository
    {
        Task CreateCustomerAsync(Customer customer);

        Task<List<Customer>> GetCustomersAsync(string? city);

        Task<long> GetTotalDocumentsCountAsync();
    }
}
