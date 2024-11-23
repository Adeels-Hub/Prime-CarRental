using PrimeGroup.CarRentalService.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace PrimeGroup.CarRentalService.Api.RequestDTOs
{
    public class VehicleAvailabilityRequest
    {
        //.Net built in Required field validator works with value types only if they are nullable
        [ValidateRequiredDate]
        [DataType(DataType.DateTime, ErrorMessage = "Pickup date must be a valid date and time.")]
        public DateTime PickupDate { get; set; }

        //.Net built in Required field validator works with value types only if they are nullable
        [ValidateRequiredDate]
        [DataType(DataType.DateTime, ErrorMessage = "Return date must be a valid date and time.")]
        [CompareDates(nameof(PickupDate), ErrorMessage = "Return date must be later than the pickup date.")]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// Comma-separated list of vehicle types.
        /// </summary>
        public string? VehicleTypes { get; set; }

        /// <summary>
        /// Converts the comma-separated vehicle types into an array.
        /// </summary>
        public string[]? GetVehicleTypesArray()
        {
            return string.IsNullOrEmpty(VehicleTypes)
                ? null
                : VehicleTypes.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
