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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly CrearEmployeeDtoValidator _crearValidator;
        private readonly ActualizarEmployeeDtoValidator _actualizarValidator;

        public EmployeeController(
            IEmployeeService employeeService,
            IMapper mapper,
            CrearEmployeeDtoValidator crearValidator,
            ActualizarEmployeeDtoValidator actualizarValidator)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Dto-AutoMapper

        // Consultar todos los empleados
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetEmployeesDtoMapper()
        {
            var employees =
                await _employeeService.GetAllEmployeesAsync();

            var employeesDto =
                _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            var response =
                new ApiResponse<IEnumerable<EmployeeDto>>(employeesDto);

            return Ok(response);
        }

        // Consultar empleado por ID
        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetEmployeeByIdDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            var employee =
                await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                throw new BusinessException(
                    "El empleado no existe.",
                    HttpStatusCode.NotFound);
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            var response = new ApiResponse<EmployeeDto>(employeeDto);

            return Ok(response);
        }

        // Registrar empleado
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> InsertEmployeeDtoMapper(
            [FromBody] EmployeeDto employeeDto)
        {
            var validationResult =
                await _crearValidator.ValidateAsync(employeeDto);

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

            var employee = _mapper.Map<Employee>(employeeDto);

            await _employeeService.InsertEmployee(employee);

            var resultDto = _mapper.Map<EmployeeDto>(employee);

            return CreatedAtAction(
                nameof(GetEmployeeByIdDtoMapper),
                new { id = employee.Id },
                resultDto);
        }

        // Actualizar empleado
        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateEmployeeDtoMapper(
            int id,
            [FromBody] EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                throw new BusinessException(
                    "El ID de la ruta no coincide con el ID del empleado.");
            }

            var validationResult =
                await _actualizarValidator.ValidateAsync(employeeDto);

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

            var existingEmployee =
                await _employeeService.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                throw new BusinessException(
                    "El empleado no existe.",
                    HttpStatusCode.NotFound);
            }

            _mapper.Map(employeeDto, existingEmployee);

            await _employeeService.UpdateEmployee(existingEmployee);

            var resultDto =
                _mapper.Map<EmployeeDto>(existingEmployee);

            return Ok(resultDto);
        }

        // Desactivar empleado
        [HttpPatch("dto/mapper/{id}/deactivate")]
        public async Task<IActionResult> DeactivateEmployeeDtoMapper(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException(
                    "El ID debe ser mayor que cero.");
            }

            await _employeeService.DeactivateEmployee(id);

            return NoContent();
        }

        #endregion
    }
}