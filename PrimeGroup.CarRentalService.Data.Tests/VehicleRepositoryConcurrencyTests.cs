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
            // Arrange: Set up reservation dates and vehicle type
            var pickupDate = DateTime.Now.AddDays(1);
            var returnDate = DateTime.Now.AddDays(3);
            var vehicleType = "Compact";

            // Pass the specific vehicleType in the last parameter
            var vehicleTypes = new[] { vehicleType };
            var initialStock = (await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes))[vehicleType];
            Assert.True(initialStock > 0, "Initial stock should be greater than 0 for this test.");

            // Number of threads simulating concurrent users
            var numberOfUsers = 100;

            // List to track reservation results
            var reservationResults = new ConcurrentBag<bool>();
            var tasks = new List<Task>();

            // Act: Run multiple threads calling GetAvailableVehiclesAsync and AddReservationAsync concurrently
            for (int i = 0; i < numberOfUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    // Attempt to reserve the vehicle
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

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // Assert: Check the results
            var successfulReservations = reservationResults.Count(r => r);
            var finalStock = (await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, returnDate, vehicleTypes))[vehicleType];

            // Ensure only one car was reserved for each available stock unit
            Assert.Equal(initialStock, successfulReservations);
            Assert.Equal(0, finalStock); // Final stock should be zero
        }


    }
}
