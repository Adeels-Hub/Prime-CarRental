using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using System.Collections.Concurrent;

/// <summary>
/// VehicleRepository will be used to work with all vehicle-related data.
/// In DDD, Repositories are per aggregate root, not per table. 
/// If the project scope changes, Reservation may itself become a candidate for a separate aggregate root.
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    // _totalStock is static and only accessed at startup to populate _availableStock, which is also static.
    // If requirements change and _totalStock needs modification after application start, its type should be reviewed.
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
        var availableStock = _availableStock
            .Select(v => new
            {
                VehicleType = v.Key,
                AvailableCount = v.Value - _reservations
                    .Count(r => r.VehicleType == v.Key && r.PickupDate < returnDate && r.ReturnDate > pickupDate)
            })
            .Where(v => v.AvailableCount > 0) // Exclude vehicles with no stock
            .ToDictionary(v => v.VehicleType, v => v.AvailableCount);

        // Apply vehicle type filtering if specified
        if (vehicleTypes != null && vehicleTypes.Length > 0)
        {
            availableStock = availableStock
                .Where(v => vehicleTypes.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value);
        }

        return await Task.FromResult(availableStock);
    }

    /// <summary>
    /// Add a reservation for a vehicle, ensuring thread-safe stock updates.
    /// </summary>
    /// <param name="reservation">The reservation to add.</param>
    /// <returns>A ReservationResult indicating success or failure and an error message if applicable.</returns>
    public async Task<ReservationResult> AddReservationAsync(Reservation reservation)
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

        // Deduct from available stock atomically to prevent concurrent threads from corrupting vehicle stock data
        var updatedStock = _availableStock.AddOrUpdate(
            reservation.VehicleType,
            0, // Initial value (not used here as the key already exists)
            (key, currentStock) => currentStock > 0 ? currentStock - 1 : currentStock);

        // Check if the stock was successfully updated
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
            ErrorMessage = $"Vehicle of type '{reservation.VehicleType}' was just reserved by another user. No vehicles are available now."
        };
    }
}
