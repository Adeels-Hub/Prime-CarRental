using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    /// <summary>
    /// Interface for Vehicle Repository, handling data access for vehicles and reservations.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Gets the available vehicles based on the specified pickup date, return date, and vehicle types.
        /// </summary>
        /// <param name="pickupDate">The pickup date for filtering available vehicles.</param>
        /// <param name="returnDate">The return date for filtering available vehicles.</param>
        /// <param name="vehicleTypes">Optional array of vehicle types to filter by.</param>
        /// <returns>A collection of available vehicles.</returns>
        Task<List<Vehicle>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes);

        /// <summary>
        /// Adds a reservation for a vehicle.
        /// </summary>
        /// <param name="reservation">The reservation to add.</param>
        /// <returns>A ReservationResult indicating success or failure and an error message if applicable.</returns>
        Task<ReservationResult> AddReservationAsync(Reservation reservation);
    }
}
