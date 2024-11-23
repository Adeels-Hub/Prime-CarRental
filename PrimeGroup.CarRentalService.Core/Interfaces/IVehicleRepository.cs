using PrimeGroup.CarRentalService.Core.Entities;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Dictionary<string, int>> GetAvailableVehiclesAsync();        

        Task<bool> AddReservationAsync(Reservation reservation);        
    }
}
