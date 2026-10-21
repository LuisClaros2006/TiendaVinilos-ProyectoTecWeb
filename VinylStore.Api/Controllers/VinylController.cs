using Microsoft.AspNetCore.Mvc;
using VinylStore.Core.Entities;
using VinylStore.Core.Interfaces;

namespace VinylStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinylController : ControllerBase
    {
        private readonly IVinylRepository _vinylRepository;

        public VinylController(IVinylRepository vinylRepository)
        {
            _vinylRepository = vinylRepository;
        }

        [HttpPost]
        public async Task<IActionResult> InsertVinyl(Vinyl newVinyl)
        {
            try
            {
                await _vinylRepository.InsertVinyl(newVinyl);

                return Created($"api/vinyl/{newVinyl.VinylId}", newVinyl);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al registrar el vinilo.");
            }
        }
    }
}