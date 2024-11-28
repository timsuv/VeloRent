using Spectre.Console;
using System;
using System.Collections.Generic;
using VeloRent.Functions;

namespace VeloRent.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal TotalCost { get; set; }

    public virtual Car Car { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;


    public void RentCar(Car chosenCar, Customer loggedInCustomer)
    {
        Console.WriteLine("Choose the start date for your rental: ");
        StartDate = DateOnly.Parse(Console.ReadLine());

        if (StartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            Console.WriteLine("Invalid date. Please choose a date in the future.");
            StartDate = DateOnly.Parse(Console.ReadLine());
        }

        Console.WriteLine("Choose the end date for your rental: ");
        EndDate = DateOnly.Parse(Console.ReadLine());

        if (EndDate < StartDate)
        {
            Console.WriteLine("Invalid date. Please choose a date after the start date.");
            EndDate = DateOnly.Parse(Console.ReadLine());
        }

        int timeDiff = EndDate.DayNumber - StartDate.DayNumber;
        if (timeDiff < 1)
        {
            Console.WriteLine("The rental duration must be at least 1 day.");
        }
        else
        {
            if (timeDiff == 1)
            {
                Console.WriteLine($"Rental duration chosen is {timeDiff} day.");
            }
            else
            {
                Console.WriteLine($"Rental duration chosen is {timeDiff} days.");
            }
        }

        Car = chosenCar;
        TotalCost = chosenCar.CarType.DailyRate * timeDiff;
        Console.WriteLine($"Total rental cost: {TotalCost:C} for {chosenCar.CarType.Make} {chosenCar.CarType.Model} ");

        var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Would you like to book?[/]")
                                .AddChoices("Yes", "No"));

        if (choice == "Yes")
        {
            using (var context = new VehicleRentalDbContext())
            {
                var rental = new Rental
                {
                    CarId = chosenCar.Id,
                    CustomerId = loggedInCustomer.Id,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TotalCost = TotalCost
                };

                context.Rentals.Add(rental);
                context.SaveChanges();
                Console.WriteLine("Rental successfully booked!");
            }
            

        }
        else
        {
            Console.WriteLine("Rental didn't go through");
        }
    }

}



