using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;

namespace VeloRent.Functions
{
    public class Menu
    {
        public Menu()
        {
        }
        string logo = @"
____    ____  _______  __        ______      .______       _______ .__   __. .___________.
\   \  /   / |   ____||  |      /  __  \     |   _  \     |   ____||  \ |  | |           |
 \   \/   /  |  |__   |  |     |  |  |  |    |  |_)  |    |  |__   |   \|  | `---|  |----`
  \      /   |   __|  |  |     |  |  |  |    |      /     |   __|  |  . `  |     |  |     
   \    /    |  |____ |  `----.|  `--'  |    |  |\  \----.|  |____ |  |\   |     |  |     
    \__/     |_______||_______| \______/     | _| `._____||_______||__| \__|     |__|     
                                                                                          
";
        public async Task MenuSpectre()
        {
            LoginSystem loginSystem = new LoginSystem();

            while (true)
            {
                        AnsiConsole.Write(
                         new Markup($"[green]{logo}[/]")
                             .Centered());


                AnsiConsole.Write(
                         new Markup($"[green]{"Welcome to VeloRent!"}[/]")
                             .Centered());
               

                var loggedInUser = await loginSystem.LoginAsync();
                string welcomeMessage = ($"{loginSystem.HelloMessage()} {loggedInUser.FirstName} {loggedInUser.LastName}!");

                var panel1 = new Panel(new Text(welcomeMessage)
                   .Centered())
                   .Expand()
                   .BorderStyle(Style.Parse("green"));

                AnsiConsole.Write(panel1);


                if (loggedInUser != null)
                {
                    while (true)
                    {
                        var menu = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Choose between different options:[/]")
                                .AddChoices("Rent a car", "Manage booking", "Manage profile", "Log out", "Exit the program"));

                        if (menu == "Rent a car")
                        {
                            CarRentalLocation carRentalLocation = new CarRentalLocation();
                            Console.Clear();
                            carRentalLocation.ChooseLocation(loggedInUser);
                        }
                        else if (menu == "Manage booking")
                        {
                            Console.Clear();
                            BookedCar bookedCar = new BookedCar();
                            bookedCar.BookedCarMenu(loggedInUser);
                        }
                        else if (menu == "Manage profile")
                        {
                            Console.Clear();
                            ManageCustomer manageCustomer = new ManageCustomer();
                            manageCustomer.ManageBookingMenu(loggedInUser);
                        }
                        else if (menu == "Log out")
                        {
                            Console.Clear();
                            await loginSystem.LogOut();
                            break;
                        }
                        else if (menu == "Exit the program")
                        {
                            Console.Clear();
                            Console.WriteLine("Goodbye!");
                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Login failed");
                    break;
                }

            }

        }
    }
}

