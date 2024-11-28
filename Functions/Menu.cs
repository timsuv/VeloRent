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

        public void MenuChoice()
        {
            Console.WriteLine("Welcome to VeloRent!");


            LoginSystem loginSystem = new LoginSystem();
            var loggedInUser = loginSystem.Login();

            if (loggedInUser != null)
            {


                Console.WriteLine("Choose between different options:");
                Console.WriteLine("1. Rent a car");
                Console.WriteLine("2. Check my rent");
                Console.WriteLine("3. Edit my info");


                while (true)
                {

                    Console.WriteLine("Tryck 0 för att avsluta programmeter:");

                    var input = int.TryParse(Console.ReadLine(), out var choice);
                    if (!input)
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    if (choice == 0)
                    {
                        Console.WriteLine("Porgramet avslutas...");
                        break;
                    }
                    switch (choice)
                    {
                        case 1:
                            CarRentalLocation carRentalLocation = new CarRentalLocation();
                            Rental rental = new Rental();
                             carRentalLocation.ChooseLocation(loggedInUser);
                           

                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:

                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        case 7:
                            break;

                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }

            }
            else
            {
                Console.WriteLine("Login failed");
            }
        }

    }
}
