@echo off
echo Creating solution...
dotnet new sln -n PrimeGroup.CarRentalService

echo Creating main projects...
dotnet new webapi -n PrimeGroup.CarRentalService.Api
dotnet new classlib -n PrimeGroup.CarRentalService.Core
dotnet new classlib -n PrimeGroup.CarRentalService.Data

echo Creating test projects...
dotnet new xunit -n PrimeGroup.CarRentalService.Api.Tests
dotnet new xunit -n PrimeGroup.CarRentalService.Core.Tests
dotnet new xunit -n PrimeGroup.CarRentalService.Data.Tests

echo Adding projects to solution...
dotnet sln add PrimeGroup.CarRentalService.Api
dotnet sln add PrimeGroup.CarRentalService.Core
dotnet sln add PrimeGroup.CarRentalService.Data
dotnet sln add PrimeGroup.CarRentalService.Api.Tests
dotnet sln add PrimeGroup.CarRentalService.Core.Tests
dotnet sln add PrimeGroup.CarRentalService.Data.Tests

echo Adding project references...
dotnet add PrimeGroup.CarRentalService.Api reference PrimeGroup.CarRentalService.Core
dotnet add PrimeGroup.CarRentalService.Api reference PrimeGroup.CarRentalService.Data
dotnet add PrimeGroup.CarRentalService.Data reference PrimeGroup.CarRentalService.Core
dotnet add PrimeGroup.CarRentalService.Api.Tests reference PrimeGroup.CarRentalService.Api
dotnet add PrimeGroup.CarRentalService.Api.Tests reference PrimeGroup.CarRentalService.Core
dotnet add PrimeGroup.CarRentalService.Api.Tests reference PrimeGroup.CarRentalService.Data
dotnet add PrimeGroup.CarRentalService.Core.Tests reference PrimeGroup.CarRentalService.Core
dotnet add PrimeGroup.CarRentalService.Data.Tests reference PrimeGroup.CarRentalService.Data
dotnet add PrimeGroup.CarRentalService.Data.Tests reference PrimeGroup.CarRentalService.Core

echo Done!
pause
