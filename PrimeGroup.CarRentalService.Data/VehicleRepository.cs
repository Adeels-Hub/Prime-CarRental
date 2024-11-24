using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
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
        // _totalStock is static and only accessed at startup to populate _availableStock, which is also static.
        // If requirements change and _totalStock needs modification after application start, its type should be reviewed.
        //ToDo:Use PrimeGroup.CarRentalService.Core.Entities.Vehicle for storing vehicle data
        private static readonly Dictionary<string, int> _totalStock = new()
    {
        { "Compact", 10 },
        { "Sedan", 8 },
        { "SUV", 5 },
        { "Van", 3 }
    };

        // Available stock uses a thread-safe ConcurrentDictionary. Shallow copy of _totalStock is enough
        // for protecting _totalStock from any unintending changes
        private static readonly ConcurrentDictionary<string, int> _availableStock = new(_totalStock);

        // Reservations stored in a thread-safe collection
        private static readonly ConcurrentBag<Reservation> _reservations = new();

        /// <summary>
        /// Get the available vehicles based on the provided dates and vehicle types.
        /// </summary>
        /// <param name="pickupDate">The pickup date for filtering.</param>
        /// <param name="returnDate">The return date for filtering.</param>
        /// <param name="vehicleTypes">Optional array of vehicle types to filter by.</param>
        /// <returns>A dictionary of matching vehicle types and their available stock.</returns>
        public async Task<Dictionary<string, int>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes)
        {
            //IMP: If multiple concurrent threads are calling GetAvailableVehiclesAsync as well as AddReservationAsync at 
            // the same time then there is a chance that GetAvailableVehiclesAsync returns slightly outdated data but it will
            // not crash and AddReservationAsync is implemented in such a way that user is only able to book a car if stock is 
            // actually available, no matter 100 users are trying to book the last vehicle concurrently. Last car will only be
            // reserved for only one person.            
            
            var filteredStock = (vehicleTypes != null && vehicleTypes.Length > 0) // Apply vehicle type filtering if specified
                ? _availableStock.Where(v => vehicleTypes.Contains(v.Key))
                                 .ToDictionary(v => v.Key, v => v.Value)
                : _availableStock.ToDictionary(v => v.Key, v => v.Value);

            return await Task.FromResult(filteredStock);            
        }

        private static readonly SemaphoreSlim _reservationSemaphore = new(1, 1);

        public async Task<ReservationResult> AddReservationAsync(Reservation reservation)
        {
            // Wait for access to the critical section
            await _reservationSemaphore.WaitAsync();
            try
            {
                // Check if the vehicle type exists and is available
                if (!_availableStock.TryGetValue(reservation.VehicleType, out var stock) || stock <= 0)
                {
                    return new ReservationResult
                    {
                        IsSuccessful = false,
                        ErrorMessage = $"No vehicles of type '{reservation.VehicleType}' are available."
                    };
                }

                // Deduct from available stock atomically
                var updatedStock = _availableStock.AddOrUpdate(
                    reservation.VehicleType,
                    0,
                    (key, currentStock) => currentStock > 0 ? currentStock - 1 : currentStock);

                // Ensure stock was successfully updated
                if (updatedStock < stock)
                {
                    _reservations.Add(reservation);
                    return new ReservationResult
                    {
                        IsSuccessful = true
                    };
                }

                return new ReservationResult
                {
                    IsSuccessful = false,
                    ErrorMessage = $"Vehicle of type '{reservation.VehicleType}' was just reserved by another user."
                };
            }
            finally
            {
                // Release the semaphore
                _reservationSemaphore.Release();
            }
        }


    }
}