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
    }
}
