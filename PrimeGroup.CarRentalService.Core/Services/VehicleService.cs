using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using PrimeGroup.CarRentalService.Core.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (pickupDate >= returnDate)
            {
                return new ServiceResult<Dictionary<string, int>>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pickup date must be earlier than the return date.",
                    Data = null
                };
            }

            // Delegate the filtering to the repository
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes);

            return new ServiceResult<Dictionary<string, int>>
            {
                IsSuccessful = true,
                Data = availableVehicles
            };
        }

        public async Task<ServiceResult<string>> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType)
        {
            if (pickupDate >= returnDate)
            {
                return new ServiceResult<string>
                {
                    IsSuccessful = false,
                    ErrorMessage = "Pickup date must be earlier than the return date.",
                    Data = null
                };
            }

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
