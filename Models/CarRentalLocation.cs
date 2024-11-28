using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;


namespace VeloRent.Models;

public partial class CarRentalLocation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public void ChooseLocation(Customer loggedInUSer)
    {
        //Console.WriteLine("\nChoose the location:");
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Choose the location:[/]")
                .AddChoices("Stockholm City, Kungsgatan 20", "Nacka Strand, Granitvägen 12", "Stockholm Vasastan, Odengatan 20"));

        Car chosenCar = null;

        if (choice == "Stockholm City, Kungsgatan 20")
        {
            chosenCar = ChooseCar("Stockholm City", "Kungsgatan 20");
        }
        else if (choice == "Nacka Strand, Granitvägen 12")
        {
            chosenCar = ChooseCar("Nacka Strand", "Granitvägen 12");
        }
        else 
        {
           chosenCar = ChooseCar("Stockholm Vasastan", "Odengatan 20");
        }
        if (chosenCar != null)
        {
            var rental = new Rental();
            rental.RentCar(chosenCar, loggedInUSer);
        }
    }

    public Car ChooseCar(string locationName, string locationAddress)
    {
        using (var context = new VehicleRentalDbContext())
        {
            // Fetch cars available at the specified location
            var carsAtLocation = context.CarRentalLocations
                .Where(location => location.Name == locationName && location.Address == locationAddress)
                .Include(location => location.Cars)
                .ThenInclude(car => car.CarType) // Include CarType details
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

            if (!carsAtLocation.Any())
            {
                Console.WriteLine($"No cars available at {locationName}, {locationAddress}.");
                return null;
            }

            // Display cars with AnsiConsole and let the user select one
            var selectedCar = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select a car to rent:[/]")
                    .AddChoices(carsAtLocation.Select(car =>
                        $"{car.Make} {car.Model} ({car.Type}) - {car.DailyRate:C}/day, {car.FuelType}, {car.Transmission}, Seats: {car.NumberOfSeats}")
                    )
            );

            // Find the selected car details
            var chosenCar = carsAtLocation.FirstOrDefault(car =>
                $"{car.Make} {car.Model} ({car.Type}) - {car.DailyRate:C}/day, {car.FuelType}, {car.Transmission}, Seats: {car.NumberOfSeats}" == selectedCar);

            if (chosenCar == null)
            {
                Console.WriteLine("Error selecting car. Please try again.");
                return null;
            }

            Console.WriteLine($"You selected: {chosenCar.Make} {chosenCar.Model} ({chosenCar.Type})");
            return new Car
            {
                Id = chosenCar.CarId,
                CarType = new CarType
                {
                    Make = chosenCar.Make,
                    Model = chosenCar.Model,
                    Type = chosenCar.Type,
                    DailyRate = chosenCar.DailyRate,
                    NumberOfSeats = chosenCar.NumberOfSeats,
                    FuelType = chosenCar.FuelType,
                    AutomaticOrManual = chosenCar.Transmission
                }
            };



        }
    }

}
