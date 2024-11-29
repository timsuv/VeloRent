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

      
        public async Task MenuSpectre()
        {
           

            LoginSystem loginSystem = new LoginSystem();

            while (true) 
            {
                Console.WriteLine("Welcome to VeloRent!");
                var loggedInUser = await loginSystem.LoginAsync();
                Console.WriteLine($"{loginSystem.HelloMessage()} {loggedInUser.FirstName} {loggedInUser.LastName}!");

                if (loggedInUser != null) 
                {
                    
                    while (true) 
                    {
                        var menu = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Choose between different options:[/]")
                                .AddChoices("Rent a car", "Manage booking", "Log out", "Exit the program"));

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
                        else if (menu == "Log out")
                        {
                            Console.Clear();
                            await loginSystem.LogOut(); 
                            break;
                        }
                        else if (menu == "Exit the program" )
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

    