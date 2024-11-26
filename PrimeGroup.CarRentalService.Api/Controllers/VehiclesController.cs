using Microsoft.AspNetCore.Mvc;
using PrimeGroup.CarRentalService.Api.Helpers;
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
            // ToDo: Use Fluent Validation instead of data annotation validation as the application requirements become more complex.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.CheckAvailabilityAsync(
                request.PickupDate,
                request.ReturnDate,
                request.GetVehicleTypesArray()
            );

            return Ok(ResponseHelper.CreateResponse(result));
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveVehicle([FromBody] ReservationRequest request)
        {
            // ToDo: Use Fluent Validation instead of data annotation validation as the application requirements become more complex.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.ReserveVehicleAsync(
                request.PickupDate,
                request.ReturnDate,
                request.VehicleType
            );

            return Ok(ResponseHelper.CreateResponse(result));
        }
    }
}
