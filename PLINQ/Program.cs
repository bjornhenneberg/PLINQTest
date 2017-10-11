using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLINQ
{
    public class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            RunMenu();
        }

        public static void RunMenu()
        {
            var running = true;
            do
            {
                DisplayMenu();
                var input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        new Speed().RunMenu();
                        break;
                    case 2:
                        new MathTests().RunMenu();
                        break;
                    case 3:
                        new Pictures().RunMenu();
                        break;
                    case 4:
                        new PLINQ(true).RunMenu();
                        break;
                    case 5:
                        new Fibonacci().RunMenu();
                        break;
                }
            } while (running);

        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Parallel Programming";
            Console.WriteLine("Parallel Programming");
            Console.WriteLine("");
            Console.WriteLine("1. Object Tests");
            Console.WriteLine("2. Math Tests");
            Console.WriteLine("3. Picture Tests");
            Console.WriteLine("4. PLINQ Tests");
            Console.WriteLine("5. Fibonacci Tests");
            Console.WriteLine("");
            Console.WriteLine("[0] Exit");
            Console.WriteLine();
        }

    }

    public static class IntExtension
    {
        internal static int ToInt32OrDefault(this string value, int defaultValue = 0)
        {
            int result;
            return int.TryParse(value, out result) ? result : defaultValue;
        }
    }
}
