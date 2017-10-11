using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
                    case 2:
                        PlinqTest1();
                        break;
                    case 3:
                        TaskCPUThreads();
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
            Console.WriteLine("2. Get Even Numbers");
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
            Console.WriteLine("    {0} fibonacci numbers took {1} using PLINQ", amount, parelapsed);

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

        public void PlinqTest1()
        {
            int[] source = Enumerable.Range(1, 10000000).ToArray();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Opt in to PLINQ with AsParallel.
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = source[i] * 2;
            }
            sw.Stop();
            Console.WriteLine("{1} total - {2}", source.Count(), source.Count(), sw.ElapsedMilliseconds);

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            // Opt in to PLINQ with AsParallel.
            Parallel.For(0, source.Length, item =>
            {
                source[item] = source[item] * 2;
            });

            sw2.Stop();
            Console.WriteLine("{1} total - {2}", source.Count(), source.Count(), sw2.ElapsedMilliseconds);


            Console.ReadLine();
        }

        public void TaskTest()
        {
            Console.Clear();
            Console.WriteLine("Task Test");
            var sw = Stopwatch.StartNew();

            Task t1 = Task.Factory.StartNew(Wait2Sec);
            Task t2 = Task.Factory.StartNew(Wait2Sec);
            Task.WaitAll(t1, t2);

            Console.WriteLine("Task.Factory.StartNew = " + sw.ElapsedMilliseconds / 1000.0);
            sw.Reset();
            sw.Start();

            Parallel.Invoke(Wait2Sec, Wait2Sec);
            Console.WriteLine("Parallel.Invoke = " + sw.ElapsedMilliseconds / 1000.0);
            Console.ReadLine();
        }

        public void Wait2Sec()
        {
            Thread.Sleep(2000);
        }

        public async void TaskExample()
        {
            // Create a task and supply a user delegate by using a lambda expression. 
            Task taskA = new Task(() => Console.WriteLine("Hello from taskA."));
            // Start the task.
            taskA.Start();

            // Output a message from the calling thread.
            Console.WriteLine("Hello from thread Main.");
            taskA.Wait();
            Console.ReadLine();
        }

        public async Task<int> LongRunning()
        {
            await Task.Delay(1000);
            return 1;
        }

        public void TaskCPUThreads()
        {
            Task[] taskArray = new Task[100];
            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    CustomData data = obj as CustomData;
                    if (data == null)
                        return;

                    data.ThreadNum = Thread.CurrentThread.ManagedThreadId;
                },
                                                      new CustomData() { Name = i, CreationTime = DateTime.Now.Ticks });
            }
            Task.WaitAll(taskArray);
            foreach (var task in taskArray)
            {
                var data = task.AsyncState as CustomData;
                if (data != null)
                    Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",
                                      data.Name, data.CreationTime, data.ThreadNum);
            }
            Console.ReadLine();
        }
    }
}
