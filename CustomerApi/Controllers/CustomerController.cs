using BusinessLogic.Data;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        //[OutputCache(Duration =10)] //Cache for 10 seconds
        //[OutputCache(PolicyName = "CacheForTenSeconds")]
        [OutputCache(PolicyName = "CacheByCity")]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] string? city=null)
        {
            var customers = await _customerRepository.GetCustomersAsync(city);
            var customerResponses = new List<GetCustomerResponse>();
            foreach (var customer in customers) 
            {
                var customerResponse = new GetCustomerResponse()
                {
                    Id = customer.Id,
                    LastName = customer.LastName,
                    FirstName = customer.FirstName,
                    City = customer.City,
                    Email = customer.Email,
                };
                customerResponses.Add(customerResponse);
            }
            
            return Ok(customerResponses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest createCustomerRequest)
        {
            var totalCustomerCount = await _customerRepository.GetTotalDocumentsCountAsync();
            var customer = new Customer()
            {
                Id = (totalCustomerCount + 1).ToString(),
                FirstName = createCustomerRequest.FirstName,
                LastName = createCustomerRequest.LastName,
                Email = createCustomerRequest.Email,
                City = createCustomerRequest.City
            };
            await _customerRepository.CreateCustomerAsync(customer);
            return Ok(customer.Id);
        }
    }
}
