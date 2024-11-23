namespace PrimeGroup.CarRentalService.Core.Results
{
    public class VehicleAvailabilityResult : BaseResult
    {
        /// <summary>
        /// A dictionary containing available vehicle types and their quantities.
        /// </summary>
        public Dictionary<string, int>? AvailableVehicles { get; set; }
    }
}
