
# Prime Car Rental Service

A **simple car rental service** built with **.NET 8** that provides RESTful APIs to check vehicle availability and make reservations. Designed to handle concurrency safely, ensuring data consistency even in high-load scenarios.

---

## Features

- **Vehicle Availability**: Query available vehicles for a given date range and filter by vehicle types.
- **Reservations**: Reserve vehicles while ensuring accurate stock updates in a concurrent environment.
- **Thread Safety**: Built with synchronization mechanisms like `SemaphoreSlim` and thread-safe collections (`ConcurrentDictionary` and `ConcurrentBag`).
- **RESTful API**: Includes endpoints for availability and reservation operations.
- **Validation**: Uses Data Annotations for input validation.

---

## Prerequisites

Ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A REST API testing tool like [Postman](https://www.postman.com/) or `curl`.

---

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/Adeels-Hub/Prime-CarRental.git
cd Prime-CarRental
```

### Build and Run the Project

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the project**:
   ```bash
   dotnet run --project PrimeGroup.CarRentalService.Api
   ```

4. Navigate to:
   - API documentation (Swagger UI): [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## API Endpoints

### 1. **Get Vehicle Availability**
   - **URL**: `GET /api/vehicles/availability`
   - **Query Parameters**:
     - `PickupDate` (required): Start date of the rental period.
     - `ReturnDate` (required): End date of the rental period.
     - `VehicleTypes` (optional): Array of vehicle types to filter by (e.g., `Compact`, `SUV`).
   - **Example**:
     ```bash
     curl "https://localhost:5001/api/vehicles/availability?PickupDate=2024-12-01T10:00:00&ReturnDate=2024-12-10T10:00:00&VehicleTypes=Compact,SUV"
     ```

### 2. **Reserve a Vehicle**
   - **URL**: `POST /api/vehicles/reserve`
   - **Request Body**:
     ```json
     {
       "PickupDate": "2024-12-01T10:00:00",
       "ReturnDate": "2024-12-10T10:00:00",
       "VehicleType": "Compact"
     }
     ```
   - **Example**:
     ```bash
     curl -X POST "https://localhost:5001/api/vehicles/reserve" -H "Content-Type: application/json" -d '{
       "PickupDate": "2024-12-01T10:00:00",
       "ReturnDate": "2024-12-10T10:00:00",
       "VehicleType": "Compact"
     }'
     ```

---

## Technical Highlights

### 1. **Thread-Safe Vehicle Management**
- **Available Vehicles**:
  - Stock is managed using a `ConcurrentDictionary` to prevent race conditions.
- **Reservations**:
  - Reservations are stored in a `ConcurrentBag` and synchronized using `SemaphoreSlim` to ensure atomic operations.

### 2. **Validation**
- Input requests are validated using **Data Annotations**, ensuring clean and predictable API usage.

### 3. **Error Handling**
- A global exception handler ensures consistent error responses for all endpoints.

---

## Running Tests

Unit tests verify correctness under concurrent conditions:
1. **Run the tests**:
   ```bash
   dotnet test
   ```

2. **Example Test Scenarios**:
   - Ensures only one user can book the last available vehicle under heavy load.
   - Validates correct availability updates for overlapping reservations.

---

## Contributing

1. Fork the repository.
2. Create a feature branch: `git checkout -b feature-name`.
3. Commit your changes: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature-name`.
5. Open a pull request.

---

## License

None

---

## Contact

For any questions or feedback, please contact:
- **Author**: [Adeel Suleman](https://github.com/Adeels-Hub)
- **Email**: adeel.suleman@gmail.com
