using Microsoft.AspNetCore.Mvc;
using PrimeGroup.CarRentalService.Api.RequestDTOs;
using PrimeGroup.CarRentalService.Core.Interfaces;

namespace PrimeGroup.CarRentalService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability([FromQuery] VehicleAvailabilityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.CheckAvailabilityAsync(
                request.PickupDate,
                request.ReturnDate,
                request.VehicleTypes
            );

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.AvailableVehicles); // Sends the filtered vehicles to the client
        }


        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveVehicle([FromBody] ReservationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.ReserveVehicleAsync(
                request.PickupDate,
                request.ReturnDate,
                request.VehicleType
            );

            if (!result.IsSuccessful)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Reservation successful.");
        }
    }
}
