using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLINQ
{
    class PLINQ
    {
        public Northwind dbContext = new Northwind();

        public PLINQ()
        {

        }

        public PLINQ(bool runTest)
        {
            TestDataSource();
        }

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
                        TestDataSource();
                        break;
                    case 2:
                        SelectOrdersAndDetail();
                        break;
                }
            } while (running);

        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.Title = "PLINQ Tests";
            Console.WriteLine("PLINQ Tests");
            Console.WriteLine();
            Console.WriteLine("1. Test Data Source");
            Console.WriteLine("2. Select orders and orderdetails");
            Console.WriteLine("");
            Console.WriteLine("[0] Back");
            Console.WriteLine();
        }

        public void TestDataSource()
        {
            Console.Clear();
            Console.WriteLine("Testing PLINQ Sample Data");
            Console.WriteLine();
            Console.WriteLine("Customer count: {0}", dbContext.Customers.Count());
            Console.WriteLine("Product count: {0}", dbContext.Products.Count());
            Console.WriteLine("Order count: {0}", dbContext.Orders.Count());
            Console.WriteLine("Order Details count: {0}", dbContext.Order_Details.Count());
            Console.WriteLine();
            Console.WriteLine("The Data Source looks okay.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
        }

        public void SelectOrdersAndDetail()
        {
            Console.Clear();
            Console.WriteLine("Selecting All Orders and Details");
            Console.WriteLine();
            var source = from order in dbContext.Orders select order;
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine("In Sequence");
            foreach (var n in source)
            {
                Console.WriteLine("    "+n.OrderID);
                var orderdetails = from item in dbContext.Order_Details where n.OrderID == item.OrderID select item;
                foreach (var orderdetail in orderdetails.AsParallel())
                {
                    Console.WriteLine("        {0} {1} {2} {3}",orderdetail.OrderID,orderdetail.ProductID,orderdetail.Quantity,orderdetail.UnitPrice);
                }
            }
            sw.Stop();
            long elapseds = sw.ElapsedMilliseconds;
            sw.Reset();

            Console.WriteLine("");
            Console.WriteLine("In Parallel");

            sw.Start();
            foreach (var n in source.AsParallel()) { 
                Console.WriteLine("    " + n.OrderID);
                var orderdetails = from item in dbContext.Order_Details where n.OrderID == item.OrderID select item;
                foreach (var orderdetail in orderdetails.AsParallel())
                {
                    Console.WriteLine("        {0} {1} {2} {3}", orderdetail.OrderID, orderdetail.ProductID, orderdetail.Quantity, orderdetail.UnitPrice);
                }
            };
            long elapsedp = sw.ElapsedMilliseconds;

            Console.WriteLine("Total seq query time: {0} ms", elapseds);
            Console.WriteLine("Total par query time: {0} ms", elapsedp);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
