namespace PrimeGroup.CarRentalService.Core.Results
{
    public class BaseResult
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
