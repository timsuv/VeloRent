using VeloRent.Functions;
using VeloRent.Models;

namespace VeloRent
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            Menu menu = new Menu();
           await menu.MenuSpectre();

        }

    }
}
