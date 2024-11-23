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

        public async Task<ServiceResult<Dictionary<string, int>>> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes)
        {
            // Fetch available vehicles
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync();

            // Filter by vehicle types if provided
            var filteredVehicles = vehicleTypes == null || vehicleTypes.Length == 0
                ? availableVehicles
                : availableVehicles.Where(v => vehicleTypes.Contains(v.Key))
                                   .ToDictionary(v => v.Key, v => v.Value);

            return new ServiceResult<Dictionary<string, int>>
            {
                IsSuccessful = true,
                Data = filteredVehicles
            };
        }

        public async Task<ServiceResult<string>> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType)
        {
            var reservation = new Reservation
            {
                VehicleType = vehicleType,
                PickupDate = pickupDate,
                ReturnDate = returnDate
            };

            var reservationResult = await _vehicleRepository.AddReservationAsync(reservation);

            return new ServiceResult<string>
            {
                IsSuccessful = reservationResult.IsSuccessful,
                ErrorMessage = reservationResult.IsSuccessful
                    ? null
                    : $"Unable to reserve a '{vehicleType}' as all vehicles are currently booked.",
                Data = reservationResult.IsSuccessful ? "Reservation successful!" : null
            };
        }
    }
}
