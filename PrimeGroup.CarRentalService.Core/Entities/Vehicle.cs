namespace PrimeGroup.CarRentalService.Core.Entities
{
    public class Vehicle
    {
        public string Type { get; set; } = string.Empty; // Vehicle type, e.g., "Compact", "Sedan"
        public int TotalStock { get; set; }             // Total stock available at the start
        public int AvailableStock { get; set; }         // Current available stock
    }
}
