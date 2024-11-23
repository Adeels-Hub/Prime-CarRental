using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using System.Collections.Concurrent;

/// <summary>
/// VehicleRepository will be used to work with all vehicle related data.
/// In DDD, Repositories are per aggregate root not per table. 
/// If projects scope changes then Reservation may itself become a candidate of a separate aggregrate root.
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    //_totalStock is static and only accessed at startup to populate _availableStock which is also static so we dont have to make _totalStock thread safe.
    //If requirements change and _totalStock needs modification after application start then its type should be reviewed.
    private static readonly Dictionary<string, int> _totalStock = new()
    {
        { "Compact", 10 },
        { "Sedan", 8 },
        { "SUV", 5 },
        { "Van", 3 }
    };

    // Available stock uses a thread-safe ConcurrentDictionary
    private static readonly ConcurrentDictionary<string, int> _availableStock = new(_totalStock);

    // Reservations stored in a thread-safe collection
    private static readonly ConcurrentBag<Reservation> _reservations = new();

    public Task<Dictionary<string, int>> GetAvailableVehiclesAsync()
    {
        // Create a shallow copy of _availableStock to ensure calling code cant modify its values.
        // Shallow copy is fine as keyvalue pairs are by default readonly.
        return Task.FromResult(new Dictionary<string, int>(_availableStock));
    }

    public async Task<bool> AddReservationAsync(Reservation reservation)
    {
        // Check if the vehicle type exists and is available
        if (!_availableStock.TryGetValue(reservation.VehicleType, out var stock) || stock <= 0)
        {
            return false; // No stock available
        }

        // Deduct from available stock atomically so that concurrent threads cant corrupt vehicle stock data
        var success = _availableStock.AddOrUpdate(
            reservation.VehicleType,
            0, // Initial value (not used here as the key already exists)
            (key, currentStock) => currentStock > 0 ? currentStock - 1 : currentStock);

        if (success < stock) // Ensure stock was updated successfully
        {
            _reservations.Add(reservation);
            return true;
        }

        return false; // Stock update failed
    }
}
