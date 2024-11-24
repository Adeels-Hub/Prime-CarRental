namespace PrimeGroup.CarRentalService.Core.Entities
{
    //ToDo: It will be used in future for persisting vehicles
    public class Vehicle
    {
        public string Type { get; set; } = string.Empty; // e.g., Compact, Sedan, SUV, Van
        public int TotalQuantity { get; set; }
    }
}
