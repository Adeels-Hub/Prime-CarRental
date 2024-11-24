using PrimeGroup.CarRentalService.Api.Validators;
//ToDo: Use Fluent Validation instead of data annotation validation as the application requirements become more complex.
using System.ComponentModel.DataAnnotations; 

namespace PrimeGroup.CarRentalService.Api.RequestDTOs
{
    public class VehicleAvailabilityRequest
    {
        //.Net built in Required field validator works with value types only if they are nullable
        [ValidateRequiredDate]
        [DataType(DataType.DateTime, ErrorMessage = "Pickup date must be a valid date and time.")]
        public DateTime PickupDate { get; set; } //ToDo: DateTimeOffSet is a better choice to cater for multiple time zones in Australia

        //.Net built in Required field validator works with value types only if they are nullable
        [ValidateRequiredDate]
        [DataType(DataType.DateTime, ErrorMessage = "Return date must be a valid date and time.")]
        [CompareDates(nameof(PickupDate), ErrorMessage = "Return date must be later than the pickup date.")]
        public DateTime ReturnDate { get; set; } //ToDo: DateTimeOffSet is a better choice to cater for multiple time zones in Australia

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
