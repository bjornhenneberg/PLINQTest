using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQ
{
    class MathTests
    {
        public void RunMenu()
        {
            var running = true;
            do
            {
                DisplayMenu();
                var input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 0:
                        Program.RunMenu();
                        break;
                    case 1:
                        RunTests();
                        break;
                }
            } while (running);

        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Math Tests";
            Console.WriteLine("Math Tests");
            Console.WriteLine("");
            Console.WriteLine("1. Run Math Tests");
            Console.WriteLine("");
            Console.WriteLine("[0] Exit");
            Console.WriteLine();
        }

        public void RunTests()
        {
            Console.Clear();
            Console.WriteLine("===== Math Test =====");
            Sequential();
            ParallelFor();
            Console.WriteLine("=====================");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
        }

        public void Sequential()
        {
            Console.WriteLine("    Running Sequential For Loop");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                DauntingOp(i);
                DauntingOp(i + i);
                DauntingOp(i - i);
                DauntingOp(i);
                DauntingOp(i + i);
                DauntingOp(i - i);
            }
            Console.WriteLine("        Time: {0}", sw.Elapsed);
            Console.WriteLine();
        }

        public void ParallelFor()
        {
            Console.WriteLine("    Running Parallel For Loop");
            Stopwatch sw = Stopwatch.StartNew();

            Parallel.For(0, 1000, (elem) =>
            {
                DauntingOp(elem);
                DauntingOp(elem + elem);
                DauntingOp(elem - elem);
                DauntingOp(elem);
                DauntingOp(elem + elem);
                DauntingOp(elem - elem);
            });
            Console.WriteLine("        Time: {0}", sw.Elapsed);
        }

        private void DauntingOp(int index)
        {
            try
            {
                long val = index;
                for (int i = 0; i < 1000; i++)
                {
                    long a = val + 345678;
                    long b = a + 4567;
                    long c = a - b;
                    long d = long.Parse(new Random().Next().ToString());
                    long x = d - a - b - c;
                    long y = long.Parse(new Random().Next().ToString()) - (long.Parse(new Random().NextDouble().ToString()) + 345 - x);
                }
            }
            catch { }
            finally
            {
                try
                {
                    long a = 345678;
                    long b = a + 4567;
                    long c = a - b;
                    long d = long.Parse(new Random().Next().ToString());
                    long x = d - a - b - c;
                    long y = long.Parse(new Random().Next().ToString()) - (long.Parse(new Random().Next().ToString()) + 345 - x);
                }
                catch { }
            }
        }
    }
}
