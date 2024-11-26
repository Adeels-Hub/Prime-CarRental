using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using PrimeGroup.CarRentalService.Core.Results;
using System.Collections.Concurrent;

namespace PrimeGroup.CarRentalService.Data
{
    /// <summary>
    /// VehicleRepository will be used to work with all vehicle-related data.
    /// In DDD, Repositories are per aggregate root, not per table. 
    /// If the project scope changes, Reservation may itself become a candidate for a separate aggregate root.
    /// </summary>
    public class VehicleRepository : IVehicleRepository
    {
        // Static list of vehicles representing the total stock (only initialized at startup)
        private static readonly ConcurrentDictionary<string, Vehicle> _vehicles = new()
        {
            ["Compact"] = new Vehicle { Type = "Compact", TotalStock = 10, AvailableStock = 10 },
            ["Sedan"] = new Vehicle { Type = "Sedan", TotalStock = 8, AvailableStock = 8 },
            ["SUV"] = new Vehicle { Type = "SUV", TotalStock = 5, AvailableStock = 5 },
            ["Van"] = new Vehicle { Type = "Van", TotalStock = 3, AvailableStock = 3 }
        };

        // Reservations stored in a thread-safe collection
        private static readonly ConcurrentBag<Reservation> _reservations = new();

        private static readonly SemaphoreSlim _reservationSemaphore = new(1, 1);

        /// <summary>
        /// Get the available vehicles based on the provided dates and vehicle types.
        /// </summary>
        public async Task<List<Vehicle>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes)
        {
            //IMP: If multiple concurrent threads are calling GetAvailableVehiclesAsync as well as AddReservationAsync at 
            // the same time then there is a chance that GetAvailableVehiclesAsync returns slightly outdated data but it will
            // not crash and AddReservationAsync is implemented in such a way that user is only able to book a car if stock is 
            // actually available, no matter 100 users are trying to book the last vehicle concurrently. Last car will only be
            // reserved for only one person. I preferred performance over slight temporary inconsistency in data by avodiing
            // too many calls to SemaphoreSlim

            // Filter vehicles based on the provided types
            var filteredVehicles = (vehicleTypes != null && vehicleTypes.Length > 0)
                ? _vehicles.Values.Where(v => vehicleTypes.Contains(v.Type))
                : _vehicles.Values;

            // Return the filtered list without adjusting availability directly
            return await Task.FromResult(filteredVehicles.ToList());
        }

        /// <summary>
        /// Add a reservation for a vehicle, ensuring thread-safe stock updates.
        /// </summary>
        public async Task<ReservationResult> AddReservationAsync(Reservation reservation)
        {
            // Wait for access to the critical section
            await _reservationSemaphore.WaitAsync();
            try
            {
                // Ensure the vehicle exists and has available stock
                if (!_vehicles.TryGetValue(reservation.VehicleType, out var vehicle) || vehicle.AvailableStock <= 0)
                {
                    return new ReservationResult
                    {
                        IsSuccessful = false,
                        ErrorMessage = $"No vehicles of type '{reservation.VehicleType}' are available."
                    };
                }

                // Deduct available stock
                vehicle.AvailableStock--;

                // Add the reservation
                _reservations.Add(reservation);

                return new ReservationResult
                {
                    IsSuccessful = true
                };
            }
            finally
            {
                _reservationSemaphore.Release();
            }
        }
    }
}
