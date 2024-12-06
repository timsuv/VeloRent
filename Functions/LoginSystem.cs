using Microsoft.EntityFrameworkCore;
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
                string username = String.Empty;
                while (true)
                {
                    Console.WriteLine("Enter an username: ");
                    username = Console.ReadLine();

                    if (db.Customers.Any(u => u.Username == username))
                    {
                        Console.WriteLine("Username already exists. Please try again");

                        continue;
                    }
                    break;

                }
                Console.WriteLine("\nEnter a password: ");
                string password = MaskInput().Trim();
                
                Console.WriteLine("\nEnter your first name: ");
                string firstName = Console.ReadLine().Trim();
                Console.WriteLine("\nEnter your last name: ");
                string lastName = Console.ReadLine().Trim();
                Console.WriteLine("\nEnter your email: ");
                string email = Console.ReadLine().Trim();
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

        public async Task<Customer> LoginAsync()
        {
            using (var db = new VeloRentContext())
            {
                while (true)
                {
                    Console.WriteLine("Enter your username:");
                    string username = Console.ReadLine().Trim();
                    Console.WriteLine("\nEnter your password:");
                    string password = MaskInput().Trim();

                    // Start progress bar
                    var progressTask = AnsiConsole.Progress()
                        .StartAsync(async ctx =>
                        {
                            var task = ctx.AddTask("[green]Logging in...[/]");
                            while (!ctx.IsFinished)
                            {
                                await Task.Delay(150);
                                task.Increment(10);
                            }
                        });

                    // Attempt to find the user in the database
                    var tempUser = await db.Customers
                        .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                    await progressTask;

                    // Stop progress bar

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
                        return tempUser; // Successful login
                    }
                    else
                    {
                        Console.WriteLine("Login failed: Incorrect password.");
                    }
                }
            }
        }
//           
        

        public string HelloMessage( )
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
        public string MaskInput()
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

        public async Task LogOut()
        {
            Console.WriteLine("Logging you out...\n");
            Console.WriteLine("Thank you for using VeloRent!\nSee you soon!");

            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    // Define a single progress task for logout
                    var task = ctx.AddTask("[green]Processing logout...[/]");

                    while (!ctx.IsFinished)
                    {
                        // Simulate work with progress increments
                        await Task.Delay(450); // Adjust the delay for smoother progress
                        task.Increment(10); // Increment progress by 10% each cycle
                    }
                });

            Console.WriteLine("Thank you for using VeloRent!\nSee you soon!");
            Console.Clear();


          
        }

    }
}
