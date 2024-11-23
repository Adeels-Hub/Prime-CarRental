using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    public interface IVehicleService
    {
        Task<VehicleAvailabilityResult> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes);
        Task<BaseResult> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType);
    }
}
