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

        public async Task<ServiceResult<List<Vehicle>>> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes)
        {
            // Delegate the filtering to the repository
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes);

            return new ServiceResult<List<Vehicle>>
            {
                IsSuccessful = true,
                Data = availableVehicles
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
                ErrorMessage = reservationResult.ErrorMessage,
                Data = reservationResult.IsSuccessful ? "Reservation successful!" : null
            };
        }
    }
}
