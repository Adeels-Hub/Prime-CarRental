using PrimeGroup.CarRentalService.Core.Entities;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    public interface IVehicleRepository
    {
        /// <summary>
        /// Retrieves the current available vehicles and their stock levels.
        /// </summary>
        /// <returns>A dictionary of vehicle types and their available stock.</returns>
        Task<Dictionary<string, int>> GetAvailableVehiclesAsync();

        /// <summary>
        /// Attempts to add a reservation for a vehicle.
        /// </summary>
        /// <param name="reservation">The reservation details.</param>
        /// <returns>A ReservationResult indicating success or failure and an optional error message.</returns>
        Task<ReservationResult> AddReservationAsync(Reservation reservation);
    }
}
