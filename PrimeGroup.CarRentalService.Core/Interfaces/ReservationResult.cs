using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeGroup.CarRentalService.Core.Interfaces
{
    /// <summary>
    /// Result class for adding a reservation, containing success status and an optional error message.
    /// </summary>
    public class ReservationResult
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
