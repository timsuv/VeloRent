using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VeloRent.Models;

namespace VeloRent.Functions
{
    public class LoginSystem
    {
        public LoginSystem()
        {
        }

        public void Register()
        {

            Console.WriteLine("Please fill the questions to sign up to our services!\n");



            using (var db = new VeloRentContext())
            {
                Console.WriteLine("Enter an username: ");
                string username = Console.ReadLine();

                if (db.Customers.Any(u => u.Username == username))
                {
                    Console.WriteLine("Username already exists.");
                    Register();
                    return;
                }
                Console.WriteLine("\nEnter a password: ");
                string password = MaskInput();
                Console.WriteLine("\nEnter your first name: ");
                string firstName = Console.ReadLine();
                Console.WriteLine("\nEnter your last name: ");
                string lastName = Console.ReadLine();
                Console.WriteLine("\nEnter your email: ");
                string email = Console.ReadLine();
                Console.WriteLine("\nEnter your phone number: ");
                string phoneNumber = Console.ReadLine();
                Console.WriteLine("\nEnter your license number");
                string licenseNumber = Console.ReadLine();

                string hashedPassword = PasswordHelper.HashPassword(password);

                var user = new Customer
                {
                    Username = username,
                    PasswordHash = hashedPassword,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    ContactNumber = phoneNumber,
                    DriverLicenseNumber = licenseNumber
                };
                db.Customers.Add(user);
                db.SaveChanges();
            }


        }

        public Customer Login()
        {
            using (var db = new VeloRentContext())
            {
                while (true)
                {
                    Console.WriteLine("\nEnter your username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("\nEnter your password:");
                    string password = MaskInput();

                    // Attempt to find the user in the database
                    var tempUser = db.Customers
                        .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

                    if (tempUser == null)
                    {
                        // Username not found
                        Console.WriteLine("\nLogin failed: Username not found.");
                        var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Would you like to try logging in again[/] or [blue]sign up?[/]")
                                .AddChoices("Try Again", "Sign Up"));

                        if (choice == "Sign Up")
                        {
                            Console.Clear();
                            Register();
                            // After registration, redirect back to login
                            Console.WriteLine("\nPlease log in with your new credentials.");
                            continue; // Restart login
                        }
                        else
                        {
                            Console.Clear();
                            continue; // Retry login
                        }
                    }

                    // Verify the password
                    if (PasswordHelper.VerifyPassword(password, tempUser.PasswordHash))
                    {
                        Console.Clear();
                        Console.WriteLine($"{HelloMessage()} {tempUser.FirstName} {tempUser.LastName}!");
                        return tempUser; // Successful login
                    }
                    else
                    {
                        Console.WriteLine("Login failed: Incorrect password.");
                    }
                }
            }
        }

        public string HelloMessage()
        {

            if (DateTime.Now.Hour < 12)
            {
                return "Good morning";
            }
            else if (DateTime.Now.Hour < 18)
            {
                return "Good afternoon";
            }
            else
            {
                return "Good evening";
            }
        }
        static string MaskInput()
        {
            SecureString password = new SecureString();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (!char.IsControl(key.KeyChar))
                {
                    password.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            return new System.Net.NetworkCredential(string.Empty, password).Password;

        }
    }
}
