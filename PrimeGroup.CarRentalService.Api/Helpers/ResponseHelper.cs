using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Api.Helpers
{
    public static class ResponseHelper
    {
        public static object CreateResponse<T>(T result, string? defaultSuccessMessage = "Operation successful.", string? defaultFailureMessage = "Operation failed.")
            where T : BaseResult
        {
            return new
            {
                IsSuccessful = result.IsSuccessful,
                Message = result.IsSuccessful ? defaultSuccessMessage : result.ErrorMessage ?? defaultSuccessMessage,                    
                Data = result is VehicleAvailabilityResult availability ? availability.AvailableVehicles : null
            };
        }
    }
}
