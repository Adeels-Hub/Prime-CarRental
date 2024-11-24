using PrimeGroup.CarRentalService.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace PrimeGroup.CarRentalService.Api.RequestDTOs
{
    public class ReservationRequest
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

        public string? VehicleType { get; set; }
    }
}
