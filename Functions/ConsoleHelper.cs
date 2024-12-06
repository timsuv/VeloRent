using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace VeloRent.Functions
{
   public static class ConsoleHelper
    {
        public static void CenteredText(string text, string textColor = "white")
        {
            var markup = new Markup($"[{textColor}]{text}[/]")
           .Centered();

            AnsiConsole.Write(markup);
            Console.WriteLine();
        }

        public static string CenteredInput()
        {
            int consoleWidth = Console.WindowWidth;

            // Calculate the position to center the cursor
            int padding = consoleWidth / 2;

            // Set the cursor to the center
            Console.SetCursorPosition(padding, Console.CursorTop);

            // Read and return the input
            return Console.ReadLine();
        }
       
    }
}
