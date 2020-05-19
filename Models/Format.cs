using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsConsole.Models
{
    class Format
    {

        public int validateInt(string input)
        {
            int output;
            do
            {
                if (!int.TryParse(input, out output))
                {
                    Console.Write("    Please enter a numeric value: ");
                    input = Console.ReadLine();
                }
                else if ((Convert.ToDouble(input)) <= 0)
                {
                    Console.Write("    Please enter a positive value: ");
                    input = Console.ReadLine();
                }
                else
                {
                    output = int.Parse(input);
                }
            } while ((!int.TryParse(input, out output)) || ((int.Parse(input)) <= 0));

            return output;
        }

        public int validateIntZero(string input)
        {
            int output;
            do
            {
                if (!int.TryParse(input, out output))
                {
                    Console.Write("    Please enter a numeric value, or select 0 to cancel: ");
                    input = Console.ReadLine();
                }
                else if ((Convert.ToDouble(input)) < 0)
                {
                    Console.Write("    Please enter a value greater than 0: ");
                    input = Console.ReadLine();
                }
                else
                {
                    output = int.Parse(input);
                }
            } while ((!int.TryParse(input, out output)) || ((int.Parse(input)) < 0));

            return output;
        }


        public void ViewBlogsHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    View Blog Posts\n" +
                "    ---------------------------------------------------------------------------------------------\n");
            Console.WriteLine(ViewBlogsFormat(), "Blog ID", "Blog Title");
            Console.WriteLine(ViewBlogsFormat(), "------", "------------------------------------");
            Console.ResetColor();
        }

        public void DeletePostHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    Delete Post\n" +
                "    ---------------------------------------------------------------------------------------------\n");
            Console.WriteLine(ViewBlogsFormat(), "Post ID", "Post Title");
            Console.WriteLine(ViewBlogsFormat(), "------", "------------------------------------");
            Console.ResetColor();
        }


        public void EditPostHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    ---------------------------------------------------------------------------------------------\n" +
                "    Edit Post\n" +
                "    ---------------------------------------------------------------------------------------------\n");
            Console.WriteLine(ViewBlogsFormat(), "Post ID", "Post Title");
            Console.WriteLine(ViewBlogsFormat(), "------", "------------------------------------");
            Console.ResetColor();
        }


        public void ViewBlogsSubHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ViewBlogsFormat(), "Blog ID", "Blog Title");
            Console.WriteLine(ViewBlogsFormat(), "------", "------------------------------------");
            Console.ResetColor();
        }



        public string ViewBlogsFormat()
        {
            return "    {0,-4}\t{1,-50}";
        }



    }
}
