﻿namespace PrimeGroup.CarRentalService.Core.Results
{
    //ToDo: Depending on how project grows we will change name of this class to RepositoryResult
    /// <summary>
    /// Result class for adding a reservation, containing success status and an optional error message.
    /// </summary>
    public class ReservationResult
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
