using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Core.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleAvailabilityResult> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes)
        {
            if (pickupDate >= returnDate)
            {
                return new VehicleAvailabilityResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pickup date must be earlier than the return date."
                };
            }

            // Fetch available vehicles
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync();

            // Filter by vehicle types if provided
            var filteredVehicles = vehicleTypes == null || vehicleTypes.Length == 0
                ? availableVehicles
                : availableVehicles.Where(v => vehicleTypes.Contains(v.Key))
                                   .ToDictionary(v => v.Key, v => v.Value);

            return new VehicleAvailabilityResult
            {
                IsSuccessful = true,
                AvailableVehicles = filteredVehicles
            };
        }

        public async Task<BaseResult> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType)
        {
            // Ideally fluent validation should be used
            if (pickupDate >= returnDate)
            {
                return new BaseResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pickup date must be earlier than the return date."
                };
            }

            // Create a reservation object
            var reservation = new Reservation
            {
                VehicleType = vehicleType,
                PickupDate = pickupDate,
                ReturnDate = returnDate
            };

            // Attempt to add the reservation
            var reservationResult = await _vehicleRepository.AddReservationAsync(reservation);

            return new BaseResult
            {
                IsSuccessful = reservationResult.IsSuccessful,
                ErrorMessage = reservationResult.ErrorMessage
            };
        }
    }
}
