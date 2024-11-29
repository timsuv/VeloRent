using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;

namespace VeloRent.Functions
{
    public partial class CarRentalLocation
    {
        Rental rental = new Rental();
        public void ChooseLocation(Customer loggedInUser)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Choose the location:[/]")
                    .AddChoices("Stockholm City, Kungsgatan 20", "Nacka Strand, Granitvägen 12", "Stockholm Vasastan, Odengatan 20"));

            string locationName = string.Empty;
            string locationAddress = string.Empty;

            if (choice == "Stockholm City, Kungsgatan 20")
            {
                locationName = "Stockholm City";
                locationAddress = "Kungsgatan 20";
            }
            else if (choice == "Nacka Strand, Granitvägen 12")
            {
                locationName = "Nacka Strand";
                locationAddress = "Granitvägen 12";
            }
            else
            {
                locationName = "Stockholm Vasastan";
                locationAddress = "Odengatan 20";
            }

            DateOnly startDate = GetRentalStartDate();
            DateOnly endDate = GetRentalEndDate(startDate);

            DisplayAvailableCars(locationName, locationAddress, startDate, endDate, loggedInUser);
        }

        public DateOnly GetRentalStartDate()
        {
            Console.WriteLine("Choose the start date for your rental (yyyy-MM-dd): ");
            DateOnly startDate;
            while (!DateOnly.TryParse(Console.ReadLine(), out startDate) || startDate < DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine("Invalid date. Please choose a date in the future.");
            }
            return startDate;
        }

        public DateOnly GetRentalEndDate(DateOnly startDate)
        {
            Console.WriteLine("Choose the end date for your rental (yyyy-MM-dd): ");
            DateOnly endDate;
            while (!DateOnly.TryParse(Console.ReadLine(), out endDate) || endDate < startDate)
            {
                Console.WriteLine("Invalid date. Please choose a date after the start date.");
            }
            return endDate;
        }

        private void DisplayAvailableCars(string locationName, string locationAddress, DateOnly startDate, DateOnly endDate, Customer loggedInUser)
        {
            using (var context = new VeloRentContext())
            {
                var carsAtLocation = context.CarRentalLocations
                    .Where(location => location.Name == locationName && location.Address == locationAddress)
                    .Include(location => location.Cars)
                    .ThenInclude(car => car.CarType)
                    .SelectMany(location => location.Cars.Select(car => new
                    {
                        CarId = car.Id,
                        Make = car.CarType.Make,
                        Model = car.CarType.Model,
                        Type = car.CarType.Type,
                        DailyRate = car.CarType.DailyRate,
                        NumberOfSeats = car.CarType.NumberOfSeats,
                        FuelType = car.CarType.FuelType,
                        Transmission = car.CarType.AutomaticOrManual
                    }))
                    .ToList();

                var availableCars = carsAtLocation.Where(car => !context.BookedCars.Any(bc =>
                        bc.CarId == car.CarId &&
                        startDate <= bc.EndDate &&
                        endDate >= bc.StartDate))
                    .ToList();

                if (!availableCars.Any())
                {
                    Console.WriteLine($"No cars available at {locationName}, {locationAddress} from {startDate} to {endDate}.");
                    return;
                }

                var selectedCar = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select a car to rent:[/]")
                        .AddChoices(availableCars.Select(car =>
                            $"{car.Make} {car.Model} ({car.Type}) - {car.DailyRate:C}/day, {car.FuelType}, {car.Transmission}, Seats: {car.NumberOfSeats}")
                        )
                );

                var chosenCar = availableCars.FirstOrDefault(car =>
                    $"{car.Make} {car.Model} ({car.Type}) - {car.DailyRate:C}/day, {car.FuelType}, {car.Transmission}, Seats: {car.NumberOfSeats}" == selectedCar);

                if (chosenCar == null)
                {
                    Console.WriteLine("Error selecting car. Please try again.");
                    return;
                }

                rental.RentCar(chosenCar, loggedInUser, startDate, endDate);
            }
        }



    }
}
