using PrimeGroup.CarRentalService.Core.Results;

namespace PrimeGroup.CarRentalService.Api.Helpers
{
    public static class ResponseHelper
    {
        public static object CreateResponse<T>(ServiceResult<T> result, string? defaultSuccessMessage = "Operation successful.", string? defaultFailureMessage = "Operation failed.")
        {
            return new
            {
                IsSuccessful = result.IsSuccessful,
                Message = result.IsSuccessful
                    ? (result.ErrorMessage ?? defaultSuccessMessage)
                    : (result.ErrorMessage ?? defaultFailureMessage),
                Data = result.Data
            };
        }
    }
}
