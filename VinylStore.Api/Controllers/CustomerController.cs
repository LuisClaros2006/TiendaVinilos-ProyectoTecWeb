using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VinylStore.Api.Response;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;
using VinylStore.Core.Exceptions;
using VinylStore.Services.Interfaces;
using VinylStore.Services.Validators;

namespace VinylStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly CrearCustomerDtoValidator _crearValidator;
        private readonly ActualizarCustomerDtoValidator _actualizarValidator;

        public CustomerController(
            ICustomerService customerService,
            IMapper mapper,
            CrearCustomerDtoValidator crearValidator,
            ActualizarCustomerDtoValidator actualizarValidator)
        {
            _customerService = customerService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los clientes
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetCustomersDtoMapper()
        {
            var customers = await _customerService.GetAllCustomersAsync();

            var customerDto = _mapper.Map<IEnumerable<CustomerDto>>(
                customers);

            var response = new ApiResponse<IEnumerable<CustomerDto>>(
                customerDto);

            return Ok(response);
        }

        // Consultar un cliente por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetCustomerByIdDtoMapper(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                throw new BusinessException(
                    "El cliente no existe.",
                    HttpStatusCode.NotFound);
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);

            var response = new ApiResponse<CustomerDto>(customerDto);

            return Ok(response);
        }

        // Registrar cliente
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertCustomerDtoMapper(
            [FromBody] CustomerDto customerDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(
                customerDto);

            if (!validationResult.IsValid)
            {
                throw new BusinessException(
                    "Error de validación.",
                    details: validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }).ToList());
            }

            var customer = _mapper.Map<Customer>(customerDto);

            await _customerService.InsertCustomer(customer);

            var resultDto = _mapper.Map<CustomerDto>(customer);

            return CreatedAtAction(
                nameof(GetCustomerByIdDtoMapper),
                new { id = customer.Id },
                resultDto);
        }

        // Actualizar cliente
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateCustomerDtoMapper(
            int id,
            [FromBody] CustomerDto customerDto)
        {
            if (id != customerDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del cliente.");
            }

            var validationResult = await _actualizarValidator.ValidateAsync(
                customerDto);

            if (!validationResult.IsValid)
            {
                throw new BusinessException(
                    "Error de validación.",
                    details: validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    }).ToList());
            }

            var existingCustomer = await _customerService
                .GetCustomerByIdAsync(id);

            if (existingCustomer == null)
            {
                throw new BusinessException(
                    "El cliente no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(customerDto, existingCustomer);

            await _customerService.UpdateCustomer(existingCustomer);

            var resultDto = _mapper.Map<CustomerDto>(existingCustomer);

            return Ok(resultDto);
        }

        // Desactivar cliente
        [HttpPatch("dto/mapper/{id}/deactivate")]
        public async Task<IActionResult> DeactivateCustomerDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _customerService.DeactivateCustomer(id);

            return NoContent();
        }

        #endregion
    }
}