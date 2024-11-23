namespace PrimeGroup.CarRentalService.Api.RequestDTOs
{
    public class ReservationRequest
    {
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string VehicleType { get; set; }
    }
}
