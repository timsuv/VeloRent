using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;

namespace VeloRent.Functions
{
    public partial class BookedCar
    {
        CarRentalLocation rental = new CarRentalLocation();
        public void CheckBooking(Customer loggedInUser)
        {
            using (var context = new VeloRentContext())
            {
                var bookings = context.BookedCars
                    .Where(b => b.Rental.CustomerId == loggedInUser.Id)
                    .Include(b => b.Car)
                    .ThenInclude(car => car.Location)
                    .Include(b => b.Car)
                    .ThenInclude(b => b.CarType)
                    .ToList();
                if (bookings.Count > 0)
                {
                    Console.WriteLine("Here is the history of all your bookings\n:");

                    bool upcomingHeaderPrinted = false;
                    bool pastHeaderPrinted = false;
                    bool currentHeaderPrinted = false;
                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);

                    foreach (var booking in bookings)
                    {
                        if (booking.Car != null && booking.Car.CarType != null)
                        {
                            if (booking.StartDate > today)
                            {
                                if (!upcomingHeaderPrinted)
                                {
                                    Console.WriteLine("\nUpcoming bookings");
                                    Console.WriteLine("--------------------------------");
                                    upcomingHeaderPrinted = true;
                                }
                                Console.WriteLine($"Car: {booking.Car.CarType.Make} {booking.Car.CarType.Model}, Start Date: {booking.StartDate},End Date: {booking.EndDate},  Place: {booking.Car.Location.Name}, {booking.Car.Location.Address} ");
                            }
                            else if (booking.StartDate <= today && booking.EndDate >= today)
                            {
                                if (!currentHeaderPrinted)
                                {
                                    Console.WriteLine("\nCurrent bookings:");
                                    Console.WriteLine("--------------------------------");

                                    currentHeaderPrinted = true;
                                }
                                Console.WriteLine($"Car: {booking.Car.CarType.Make} {booking.Car.CarType.Model}, Start Date: {booking.StartDate}, End Date: {booking.EndDate} Place: {booking.Car.Location.Name}, {booking.Car.Location.Address} ");
                            }
                            else if (booking.EndDate < today)
                            {
                                if (!pastHeaderPrinted)
                                {
                                    Console.WriteLine("\nPast bookings:");
                                    Console.WriteLine("--------------------------------");

                                    pastHeaderPrinted = true;
                                }
                                Console.WriteLine($"Car: {booking.Car.CarType.Make} {booking.Car.CarType.Model}, Start Date: {booking.StartDate}, End Date: {booking.EndDate} Place: {booking.Car.Location.Name}, {booking.Car.Location.Address} ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Car information not available.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("You have no bookings.");
                    return;
                }

            }




        }

        public void CancelBooking(Customer loggedInUser)
        {
            using (var context = new VeloRentContext())
            {
                var bookings = context.BookedCars
                    .Where(b => b.Rental.CustomerId == loggedInUser.Id)
                    .Include(b => b.Car)
                    .ThenInclude(c => c.CarType)
                    .Include(b => b.Rental)
                    .ToList();

                if (bookings.Count == 0)
                {
                    Console.WriteLine("You have no bookings to cancel.");
                    return;
                }

                Console.WriteLine("Your bookings:");

                var bookingChoices = bookings.Select(booking => new
                {
                    DisplayText = $"{booking.Car.CarType.Make} {booking.Car.CarType.Model}\n Start Date {booking.StartDate} \nEnd Date {booking.EndDate}",
                    Booking = booking
                }).ToList();

                // Display the car options in the selection prompt
                var bookingToCancel = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select a booking to cancel:[/]")
                        .AddChoices(bookingChoices.Select(bc => bc.DisplayText).ToList())
                );

                // Find the selected booking object based on the user's choice
                var selectedBooking = bookingChoices
                    .FirstOrDefault(bc => bc.DisplayText == bookingToCancel)?.Booking;

                if (selectedBooking != null)
                {
                    // Remove the selected booking from the database
                    context.BookedCars.Remove(selectedBooking);
                    context.SaveChanges();
                    Console.WriteLine("Booking successfully cancelled.");
                }
                else
                {
                    Console.WriteLine("Error cancelling booking.");
                }
            }
        }
        public void ChangeDate(Customer loggedInUser)
        {
            using (var context = new VeloRentContext())
            {
                var bookings = context.BookedCars
                    .Where(b => b.Rental.CustomerId == loggedInUser.Id)
                    .Include(b => b.Car)
                    .ThenInclude(b=> b.Location)
                    .Include(b => b.Car)
                    .ThenInclude(c => c.CarType)
                    .Include(b => b.Rental)
                    .ToList();

                if (bookings.Count == 0)
                {
                    Console.WriteLine("You have no bookings.");
                    return;
                }

                Console.WriteLine("Your bookings:");

                var bookingChoices = bookings.Select(booking => new
                {
                    DisplayText = $"{booking.Car.CarType.Make} {booking.Car.CarType.Model}\n Start Date {booking.StartDate} \nEnd Date {booking.EndDate}\nPlace: {booking.Car.Location.Name}\n",
                    Booking = booking
                }).ToList();

                var bookingToChange = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Select a booking where you want to change the date:[/]")
                        .AddChoices(bookingChoices.Select(bc => bc.DisplayText).ToList())
                );

                var selectedBooking = bookingChoices
                    .FirstOrDefault(bc => bc.DisplayText == bookingToChange)?.Booking;

                if (selectedBooking == null)
                {
                    Console.WriteLine("Error: selected booking not found.");
                    return;
                }

                var today = DateOnly.FromDateTime(DateTime.Now);

                var tooLate = selectedBooking.StartDate <= today.AddDays(1);
                if (tooLate)
                {
                    Console.WriteLine("It is too late to cancel ");
                    return;
                }
                var startDate = rental.GetRentalStartDate();
                var endDate = rental.GetRentalEndDate(startDate);





                var conflictBooking = context.BookedCars
                .Where(bc => bc.CarId == selectedBooking.CarId && bc.Id != selectedBooking.Id)
                  .Any(b =>
                        (startDate >= b.StartDate && startDate <= b.EndDate) ||
                      (endDate >= b.StartDate && endDate <= b.EndDate) ||
                      (startDate <= b.StartDate && endDate >= b.EndDate)

                );
                if (conflictBooking)
                {
                    Console.WriteLine("The selected dates conflict with another booking.");
                    return;
                }

                selectedBooking.StartDate = startDate;
                selectedBooking.EndDate = endDate;
                context.SaveChanges();
                Console.WriteLine("Booking date successfully changed.");




            }
        }

        public void BookedCarMenu(Customer loggedInUser)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]What would you like to do?[/]")
                    .AddChoices("Check bookings", "Cancel booking", "Change booking date", "Exit to main menu")
            );
            switch (choice)
            {
                case "Check bookings":
                    CheckBooking(loggedInUser);
                    break;
                case "Cancel booking":
                    CancelBooking(loggedInUser);
                    break;
                case "Change booking date":
                    ChangeDate(loggedInUser);
                    break;
                case "Exit to main menu":
                    return;
            }
        }
    }

}