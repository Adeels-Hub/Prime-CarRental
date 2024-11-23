namespace PrimeGroup.CarRentalService.Core.Results
{
    public class ServiceResult<T>
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// The result data, which can be of any type.
        /// </summary>
        public T? Data { get; set; }
    }

}
