using Microsoft.AspNetCore.Mvc;
using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;

namespace VinylStore.Api.Controllers
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


        [HttpPost]
        public async Task<IActionResult> InsertCustomer(Customer newCustomer)
        {
            try
            {
                await _customerRepository.InsertCustomer(newCustomer);
                return Created($"api/customer/{newCustomer.CustomerId}", newCustomer);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al registrar el cliente.");
            }
        }

    }
}
