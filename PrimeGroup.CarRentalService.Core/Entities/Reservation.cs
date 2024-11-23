namespace PrimeGroup.CarRentalService.Core.Entities
{
    public class Reservation
    {
        public string VehicleType { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
