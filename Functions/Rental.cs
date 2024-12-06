using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;
using Microsoft.EntityFrameworkCore;

namespace VeloRent.Models
{
    public partial class Rental
    {
        public void RentCar(dynamic chosenCar, Customer loggedInUser, DateOnly startDate, DateOnly endDate)
        {
            int timeDiff = endDate.DayNumber - startDate.DayNumber;
            decimal totalCost = chosenCar.DailyRate * timeDiff;
            Console.WriteLine($"You selected: {chosenCar.Make} {chosenCar.Model} ({chosenCar.Type})");
            Console.WriteLine($"Total rental cost: {totalCost:C}");



            var choice = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("[green]Would you like to book?[/]")
                                        .AddChoices("Yes", "No"));

            if (choice == "Yes")
            {
                using (var context = new VeloRentContext())
                {
                    var rental = new Rental
                    {
                        CarId = chosenCar.CarId,
                        CustomerId = loggedInUser.Id,
                        StartDate = startDate,
                        EndDate = endDate,
                        TotalCost = totalCost
                    };

                    context.Rentals.Add(rental);
                    context.SaveChanges();

                    var bookedCar = new BookedCar
                    {
                        CarId = chosenCar.CarId,
                        StartDate = startDate,
                        EndDate = endDate,
                        RentalId = rental.Id
                    };

                    context.BookedCars.Add(bookedCar);
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
}
