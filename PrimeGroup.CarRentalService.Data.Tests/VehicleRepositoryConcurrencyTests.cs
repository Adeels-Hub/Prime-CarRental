using PrimeGroup.CarRentalService.Core.Entities;
using PrimeGroup.CarRentalService.Core.Interfaces;
using System.Collections.Concurrent;

namespace PrimeGroup.CarRentalService.Data.Tests
{
    /// <summary>
    /// Contains tests to verify that the VehicleRepository class handles concurrency safely.
    /// </summary>
    public class VehicleRepositoryConcurrencyTests
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleRepositoryConcurrencyTests()
        {
            _vehicleRepository = new VehicleRepository();
        }

        /// <summary>
        /// Tests that only one reservation is successful when multiple threads attempt 
        /// to reserve the last available vehicle concurrently.
        /// </summary>
        /// <returns>A Task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task ConcurrentAccess_ShouldAllowOnlyOneReservation_ForLastVehicle()
        {
            // Arrange
            var pickupDate = DateTime.Now.AddDays(1);
            var returnDate = DateTime.Now.AddDays(3);
            var vehicleType = "Compact";

            // Fetch available vehicles and confirm stock
            var vehicleTypes = new[] { vehicleType };
            var availableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes);
            var initialStock = availableVehicles.FirstOrDefault(v => v.Type == vehicleType)?.AvailableStock ?? 0;

            Assert.True(initialStock > 0, $"No vehicles of type '{vehicleType}' are available for testing.");

            // Simulate concurrent reservations
            var numberOfUsers = 100;
            var reservationResults = new ConcurrentBag<bool>();
            var tasks = new List<Task>();

            for (int i = 0; i < numberOfUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var reservation = new Reservation
                    {
                        VehicleType = vehicleType,
                        PickupDate = pickupDate,
                        ReturnDate = returnDate
                    };
                    var result = await _vehicleRepository.AddReservationAsync(reservation);
                    reservationResults.Add(result.IsSuccessful);
                }));
            }

            // Act
            await Task.WhenAll(tasks);

            // Assert
            var successfulReservations = reservationResults.Count(r => r);
            var finalAvailableVehicles = await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes);
            var finalStock = finalAvailableVehicles.FirstOrDefault(v => v.Type == vehicleType)?.AvailableStock ?? 0;

            Assert.Equal(initialStock, successfulReservations);
            Assert.Equal(0, finalStock);
        }

    }
}
