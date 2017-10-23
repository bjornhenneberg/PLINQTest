using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQ
{
    public class Synopsis
    {
        static Synopsis()
        {
            var temp = GetArray(100000000);

            Console.WriteLine("=== Simple Data Parallelism ===");
            Console.WriteLine();
            Console.WriteLine("    Sequential doubling of array");
            Console.Write("        Time: ");
            DoubleSeq(temp);
            Console.WriteLine();
            Console.WriteLine("    Parallel doubling of array");
            Console.Write("        Time: ");
            DoubleParal(temp);
            Console.WriteLine();
            Console.WriteLine("    Parallel doubling of partitioned array");
            Console.Write("        Time: ");
            DoubleParalPart(temp);
            Console.WriteLine();
            Console.WriteLine("=======================================");

            Console.WriteLine("Press to continue.");
            Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("========== ForEach vs ForAll ==========");
            Console.WriteLine();
            Console.WriteLine("    ForEach PLINQ");
            Console.Write("        Time: ");
            ForEachPlinq();
            Console.WriteLine();
            Console.WriteLine("    ForAll PLINQ");
            Console.Write("        Time: ");
            ForAllPlinq();
            Console.WriteLine();
            Console.WriteLine("=======================================");

            Console.WriteLine("Press to continue.");
            Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("========= Ordered vs Unordered ========");
            Console.WriteLine();
            Console.WriteLine("    Ordered PLINQ");
            Console.Write("        Time: ");
            AsOrderedPlinq();
            Console.WriteLine();
            Console.WriteLine("    Unordered PLINQ");
            Console.Write("        Time: ");
            AsUnorderedPlinq();
            Console.WriteLine();
            Console.WriteLine("=======================================");

            Console.WriteLine("Press to continue.");
            Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("======== Degrees of Parallism =========");
            Console.WriteLine("    One core paral PLINQ");
            Console.Write("        Time: ");
            OneCoreParal();
            Console.WriteLine();
            Console.WriteLine("    Four cores paral PLINQ");
            Console.Write("        Time: ");
            FourCoreParal();
            Console.WriteLine();
            Console.WriteLine("=======================================");

            Console.ReadLine();

        }

        /// <summary>
        /// Get an array of size i
        /// </summary>
        /// <param name="i">Size of array</param>
        /// <returns>Array of random numbers in i size</returns>
        static int[] GetArray(int i)
        {
            var temp = new List<int>();
            var ran = new Random();
            for (int j = 0; j < i; j++)
            {
                temp.Add(ran.Next(1000));
            }
            return temp.ToArray();
        }

        // Simple Data Parallelism
        /// <summary>
        /// Normal for loop for doubling each number of an array
        /// </summary>
        /// <param name="array">array to use</param>
        static void DoubleSeq(int[] array)
        {
            var watch = Stopwatch.StartNew();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= 2;
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        /// <summary>
        /// Parallel for loop for doubling each number of an array
        /// </summary>
        /// <param name="array">array to use</param>
        static void DoubleParal(int[] array)
        {
            var watch = Stopwatch.StartNew();

            Parallel.For(0, array.Length, i =>
            {
                array[i] *= 2;
            });

            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        /// <summary>
        /// Parallel foreach with partitions of an array
        /// </summary>
        /// <param name="array">array to use</param>
        static void DoubleParalPart(int[] array)
        {
            var watch = Stopwatch.StartNew();

            Parallel.ForEach(Partitioner.Create(0, array.Length),
            (range) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    array[i] *= 2;
                }
            });

            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        // ForEach vs ForAll using PLINQ
        /// <summary>
        /// 
        /// </summary>
        static void ForEachPlinq()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 10000);
            var query = from num in nums.AsParallel()
                        where num % 10 == 0
                        select num;
            var con = new List<int>();
            foreach (var i in query)
            {
                con.Add(i);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        /// <summary>
        /// 
        /// </summary>
        static void ForAllPlinq()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 10000);
            var query = from num in nums.AsParallel()
                        where num % 10 == 0
                        select num;
            var con = new ConcurrentBag<int>();
            query.ForAll(e => con.Add(e));
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        // Ordered vs Unordered query using PLINQ
        /// <summary>
        /// 
        /// </summary>
        static void AsOrderedPlinq()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 100000);
            var query = from num in nums.AsParallel().AsOrdered()
                        where num % 10 == 0
                        select num;
            var con = new List<int>();
            foreach (var i in query)
            {
                con.Add(i);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        /// <summary>
        /// 
        /// </summary>
        static void AsUnorderedPlinq()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 100000);
            var query = from num in nums.AsParallel().AsUnordered()
                        where num % 10 == 0
                        select num;
            var con = new List<int>();
            foreach (var i in query)
            {
                con.Add(i);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        //Degrees of Parallelization with PLINQ
        /// <summary>
        /// - Takes the time and outputs it
        /// - Creates an array of ints from 10 to 10000 and querys that for all numbers divisible with 10 and adds them to a list
        /// - Uses PLINQ to limit the degree of parallelism to 1
        /// </summary>
        static void OneCoreParal()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 10000);
            var query = from num in nums.AsParallel().WithDegreeOfParallelism(1)
                        where num % 10 == 0
                        select num;
            var con = new List<int>();
            foreach (var i in query)
            {
                con.Add(i);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        /// <summary> 
        /// - Takes the time and outputs it
        /// - Creates an array of ints from 10 to 10000 and querys that for all numbers divisible with 10 and adds them to a list
        /// - Uses PLINQ to limit the degree of parallelism to 4
        /// </summary>
        static void FourCoreParal()
        {
            var watch = new Stopwatch();
            watch.Start();
            var nums = Enumerable.Range(10, 10000);
            var query = from num in nums.AsParallel().WithDegreeOfParallelism(4)
                        where num % 10 == 0
                        select num;
            var con = new List<int>();
            foreach (var i in query)
            {
                con.Add(i);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }
    }
}
