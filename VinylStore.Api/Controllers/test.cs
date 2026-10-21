
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinylStore.Infrastructure.Data;

namespace VinylStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseTestController : ControllerBase
    {
        private readonly VinylStoreContext _context;

        public DatabaseTestController(VinylStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                bool isConnected = await _context.Database
                    .CanConnectAsync();

                if (isConnected)
                {
                    return Ok(new
                    {
                        message = "Conexion exitosa con MySQL"
                    });
                }

                return StatusCode(500, new
                {
                    message = "No se pudo conectar a MySQL"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "Error al conectar con la base de datos"
                });
            }
        }
    }
}
