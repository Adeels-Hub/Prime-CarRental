namespace PrimeGroup.CarRentalService.Api.RequestDTOs
{
    public class VehicleAvailabilityRequest
    {
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string[]? VehicleTypes { get; set; }
    }
}
