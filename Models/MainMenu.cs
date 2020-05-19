using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsConsole.Models
{
    class MainMenu
    {
        public MainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    Welcome to Matts Blog!!\n" +
                "    ---------------------------------------------------------------------------------------------\n");
            Console.ResetColor();
            Console.WriteLine("    What would you like to do?\n\n" +
                "    1) Display all blogs\n" +
                "    2) Add Blog\n" +
                "    3) Create Post\n" +
                "    4) Display Posts\n" +
                "    5) Delete Post\n" +
                "    6) Edit Post\n" +
                "    7) Exit ");

            Console.Write("    ");
        }
        public int GetMainMenuInpput()
        {
            Format i = new Format();
            int selection;

            selection = i.validateInt(Console.ReadLine());

            while ((selection < 0 || selection > 7))
            {
                Console.Write("    Please Enter a valid response 1 - 7 ");
                selection = i.validateInt(Console.ReadLine());
            }
            return selection;
        }
    }
}
