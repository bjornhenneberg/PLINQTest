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
    class Pictures
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
                        PictureTest();
                        break;
                }
            } while (running);

        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Product Images to SQL");
            Console.WriteLine();
            Console.WriteLine("1. Pictures Test");
            Console.WriteLine("");
            Console.WriteLine("[0] Back");
            Console.WriteLine();
        }

        public void PictureTest()
        {
            Console.Clear();
            Console.WriteLine("===== Process Pictures =====");
            Console.WriteLine();
            Console.WriteLine("Please input a directory with images (ex. 'C:\\Users\\NAME\\Pictures') ");
            var dirInput = Console.ReadLine();
            dirInput = dirInput.Replace("\"", "");
            Console.Clear();
            Console.WriteLine("===== Process Pictures =====");
            Console.WriteLine();
            try
            {
                String[] files = System.IO.Directory.GetFiles(dirInput, "*.jpg");

                Console.WriteLine("Found {0} files, do you want to continue?  (Y)es  (n)o", files.Length);
                var response = Console.ReadLine();
                if (response == "n")
                {
                    RunMenu();
                }
                String newDir = dirInput + @"\Modified";
                System.IO.Directory.CreateDirectory(newDir);

                Console.Clear();
                Console.WriteLine("===== Process Pictures =====");
                Console.WriteLine("    Running Parallel For Loop");
                var sw = new Stopwatch();
                sw.Start();
                Parallel.ForEach(files, (currentFile) =>
                {
                    String filename = System.IO.Path.GetFileName(currentFile);
                    var bitmap = new Bitmap(currentFile);

                    bitmap.Save(Path.Combine(newDir, filename));

                    if (filename.Length > 30)
                        filename = filename.Substring(0, 30);
                    Console.Write("\r        Processing {0} on thread {1} ", filename, Thread.CurrentThread.ManagedThreadId);
                });
                var paralleltime = sw.ElapsedMilliseconds;

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("    Running Sequential For Loop");
                foreach (var item in files)
                {
                    String filename = System.IO.Path.GetFileName(item);
                    var bitmap = new Bitmap(item);

                    bitmap.Save(Path.Combine(newDir, filename));

                    if (filename.Length > 30)
                        filename = filename.Substring(0, 30);
                    Console.Write("\r        Processing {0} on thread {1}", filename, Thread.CurrentThread.ManagedThreadId);
                }
                var sequentialtime = sw.ElapsedMilliseconds;

                // Keep the console window open in debug mode.
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Parallel Time Elapsed: " + paralleltime);
                Console.WriteLine("Sequential Time Elapsed: " + sequentialtime);
                Console.WriteLine("============================");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            catch (Exception)
            {
                Console.WriteLine("That doesnt look like a directory");
                Console.ReadLine();
                RunMenu();
            }
        }
    }
}
