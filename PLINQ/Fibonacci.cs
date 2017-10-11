using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQ
{
    public class Fibonacci
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
                        CalculateFibonacci();
                        break;
                }
            } while (running);

        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Fibonacci";
            Console.WriteLine("Fibonacci Tests");
            Console.WriteLine();
            Console.WriteLine("1. Calculate Fibonacci Numbers");
            Console.WriteLine();
            Console.WriteLine("[0] Back");
            Console.WriteLine();
        }

        public void CalculateFibonacci()
        {
            Console.Clear();
            var amount = 100;
            Console.WriteLine("Please input the amount of numbers to calculate.");
            amount = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            double[] arr = new double[amount];

            for (int i = 0; i < amount; i++)
            {
                arr[i] = i;
            }

            //As Parallel
            Console.WriteLine("Calculating in Parallel");
            Stopwatch sw = new Stopwatch();
            var fibonacciNumbers = arr.AsParallel().Select(n => computeFibonacci(n)).OrderBy(x => x);
            int index = 0;
            sw.Start();
            foreach (var number in fibonacciNumbers)
            {
                //Console.Write("\r {0} : {1}",index,number);
                //index++;
            }
            var parelapsed = sw.Elapsed;
            Console.WriteLine("    {0} fibonacci numbers took {1} using PLINQ", amount,parelapsed);

            Console.WriteLine();

            //As Sequential
            Console.WriteLine("Calculating in Sequence");
            Stopwatch sw2 = new Stopwatch();
            var fibonacciNumbers2 = arr.Select(n => computeFibonacci(n)).OrderBy(x => x);
            int index2 = 0;
            sw2.Start();
            foreach (var number in fibonacciNumbers2)
            {
                //Console.Write("\r {0} : {1}", index2, number);
                //index2++;
            }
            var seqelapsed = sw2.Elapsed;
            Console.WriteLine("    {0} fibonacci numbers took {1} using LINQ", amount, seqelapsed);
            Console.WriteLine();
            Console.Write("Press any key to continue.");
            Console.ReadLine();
        }

        public double computeFibonacci(double n)
        {
            double a = 0;
            double b = 1;
            // In N steps compute Fibonacci sequence iteratively.
            for (double j = 0; j < n; j++)
            {
                double temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}
