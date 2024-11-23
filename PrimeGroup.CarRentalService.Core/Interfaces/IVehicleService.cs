using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    public interface IVehicleService
    {
        Task<ServiceResult<Dictionary<string, int>>> CheckAvailabilityAsync(DateTime pickupDate, DateTime returnDate, string[]? vehicleTypes);
        Task<ServiceResult<string>> ReserveVehicleAsync(DateTime pickupDate, DateTime returnDate, string vehicleType);
    }
}
