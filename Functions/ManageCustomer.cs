using Microsoft.Identity.Client;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;

namespace VeloRent.Functions
{
    public class ManageCustomer : Customer
    {
        public void ChangeUsername(Customer loggedInUser)
        {
            Console.WriteLine("Enter new username: ");

            using (var db = new VeloRentContext())
            {
                string newUsername = String.Empty;
                while (true)
                {

                    newUsername = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newUsername))
                    {
                        Console.WriteLine("Username cannot be empty or whitespace. Please try again.");
                        continue;
                    }

                    if (db.Customers.Any(u => u.Username == newUsername))
                    {
                        Console.WriteLine("Username already exists. Please try again");

                        continue;
                    }
                    break;
                }
                var user = db.Customers.FirstOrDefault(u => u.Id == loggedInUser.Id);
                if (user != null)
                {
                    user.Username = newUsername;
                    db.SaveChanges();
                    Console.WriteLine("Username successfully updated!");
                }
                else
                {
                    Console.WriteLine("Unable to find the logged-in user in the database.");
                }

            }



        }
        public void ChangePassword(Customer loggedInUser)
        {
            using (var db = new VeloRentContext())
            {
                var inputPass = new LoginSystem();
                bool isPasswordUpdated = false;
                while (!isPasswordUpdated)
                {
                    Console.WriteLine("Enter current password");

                    string currentPassword = inputPass.MaskInput().Trim();
                    Console.WriteLine();

                    if (PasswordHelper.VerifyPassword(currentPassword, loggedInUser.PasswordHash))
                    {
                        string newPassword = String.Empty;

                        bool isPasswordSame = true;
                        while (isPasswordSame)
                        {
                            Console.WriteLine("Enter a new password");
                            newPassword = inputPass.MaskInput().Trim();
                            Console.WriteLine();
                           
                            if (newPassword == currentPassword)
                            {
                                Console.WriteLine("New password cannot be the same as the current password. Please try again.");

                            }
                            else
                            {
                                isPasswordSame = false;
                            }
                        }

                        string hashedPassword = PasswordHelper.HashPassword(newPassword);

                        var user = db.Customers.FirstOrDefault(u => u.Id == loggedInUser.Id);
                        if (user != null)
                        {
                            user.PasswordHash = hashedPassword;
                            db.SaveChanges();
                            Console.WriteLine("\nPassword successfully updated!");
                            isPasswordUpdated = true;
                        }
                        else
                        {
                            Console.WriteLine("Unable to find the logged-in user in the database.");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password. Try again");

                    }

                }

            }
        }
        public void ManageBookingMenu(Customer loggedInUser)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]What would you like to do?[/]")
                    .AddChoices("Change username", "Change password", "Exit to main menu")
            );
            switch (choice)
            {
                case "Change username":
                    ChangeUsername(loggedInUser);
                    break;
                case "Change password":
                    ChangePassword(loggedInUser);
                    break;
                case "Exit to main menu":
                    return;
            }
        }
    }
}
