using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    /// <summary>
    /// Interface for Vehicle Service, handling business logic for vehicles and reservations.
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Checks the availability of vehicles based on the provided pickup date, return date, and vehicle types.
        /// </summary>
        /// <param name="pickupDate">The pickup date for availability checking.</param>
        /// <param name="returnDate">The return date for availability checking.</param>
        /// <param name="vehicleTypes">Optional array of vehicle types to filter by.</param>
        /// <returns>A ServiceResult containing a list of available vehicles.</returns>
        Task<ServiceResult<List<Vehicle>>> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes);

        /// <summary>
        /// Reserves a vehicle of the specified type for the given date range.
        /// </summary>
        /// <param name="pickupDate">The pickup date for the reservation.</param>
        /// <param name="returnDate">The return date for the reservation.</param>
        /// <param name="vehicleType">The type of vehicle to reserve.</param>
        /// <returns>A ServiceResult containing the reservation status and any applicable message.</returns>
        Task<ServiceResult<string>> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType);
    }
}
