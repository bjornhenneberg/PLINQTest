using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using RestSharp;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PLINQ
{
    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Location
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public object postcode { get; set; }
    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string md5 { get; set; }
        public string sha1 { get; set; }
        public string sha256 { get; set; }
    }

    public class Id
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }

    public class User
    {
        public string gender { get; set; }
        public Name name { get; set; }
        public Location location { get; set; }
        public string email { get; set; }
        public Login login { get; set; }
        public DateTime dob { get; set; }
        public DateTime registered { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public Id id { get; set; }
        public Picture picture { get; set; }
        public string nat { get; set; }
    }

    public class Info
    {
        public string seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string version { get; set; }
    }

    public class RootObject
    {
        public List<User> results { get; set; }
        public Info info { get; set; }
    }

    public class Speed
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
                        DoUserTest();
                        break;
                }
            } while (running);

        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.Title = "Speed Tests";
            Console.WriteLine("Speed Test");
            Console.WriteLine("");
            Console.WriteLine("1. Do User Test");
            Console.WriteLine("");
            Console.WriteLine("[0] Exit");
            Console.WriteLine();
        }

        public void DoUserTest()
        {
            int webamount = 250;
            int diskamount = 10000;

            List<User> userlist = new List<User>();

            Console.Clear();
            Console.WriteLine("===== User Tests ======");
            Console.WriteLine();

            Console.WriteLine("--- Net based tests ---");
            userlist = GetUsers(webamount, true, true);
            GetUsers(webamount, true, false);
            Console.WriteLine("-----------------------");
            Console.WriteLine();
            Console.WriteLine("--- Disk based tests --");
            //GetUsers(diskamount, false, true);
            //GetUsers(diskamount, false, false);
            Console.WriteLine("-----------------------");
            Console.WriteLine();

            userlist = userlist.Where(x => x != null).ToList();

            Console.WriteLine("--- Select Tests ------");
            Console.WriteLine("    List containes {0} users", userlist.Count);
            Console.WriteLine();
            DateTime overRepos = DateTime.ParseExact("1960-01-01 00:00:00","yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            int overPostCode = 0;

            Console.WriteLine("    As Parallel");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            List<User> parallel = userlist
                .AsParallel()
                    .Where(user => user.dob > overRepos && IntExtension.ToInt32OrDefault(user.location.postcode.ToString()) > overPostCode).ToList();
            Console.WriteLine("        Time: {0}", sw.Elapsed);
            Console.WriteLine("            Parallel list contains {0} users",parallel.Count);
            foreach (var item in parallel)
            {
            }

            Console.WriteLine();

            Console.WriteLine("    As Non-Parallel");
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            List<User> nonparallel = userlist
                .Where(user => user.dob > overRepos && IntExtension.ToInt32OrDefault(user.location.postcode.ToString()) > overPostCode).ToList();
            Console.WriteLine("        Time: {0}", sw2.Elapsed);
            Console.WriteLine("            Non-Parallel list contains {0} users", nonparallel.Count);
            foreach (var item in nonparallel)
            {
            }
            Console.WriteLine("-----------------------");

            Console.WriteLine();
            Console.WriteLine("=======================");
            Console.ReadLine();
        }

        public List<User> GetUsers(int amount, bool fromWeb, bool asParallel)
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine("    Getting {0} Users | Using web: {1} | AsParallel: {2}", amount, fromWeb.ToString(), asParallel.ToString());

            sw.Start();
            if (asParallel)
            {
                List<User> parallellist = new List<User>();
                
                //Parallel For
                Parallel.For(0, amount, i =>
                {
                    User user = InitUser(fromWeb);
                    parallellist.Add(user);
                    Console.Title = "    Parallel - User: " + user.id.name + user.id.value + " | Index: " + i + " | Thread: " + Thread.CurrentThread.ManagedThreadId;
                    //Console.WriteLine("    Parallel - User: {0} | Index: {1} | Thread: {2}", user.id.name + user.id.value, i, Thread.CurrentThread.ManagedThreadId);
                });
                Console.WriteLine("        Time: {0}", sw.Elapsed);
                Console.WriteLine();
                return parallellist;
            }
            else
            {
                List<User> nonparallellist = new List<User>();

                //Non-Parallel For
                for (int i = 0; i < amount; i++)
                {
                    User user = InitUser(fromWeb);
                    nonparallellist.Add(user);
                    Console.Title = "Non-Parallel - User: " + user.id.name + user.id.value + " | Index: " + i + " | Thread: " + Thread.CurrentThread.ManagedThreadId;
                    //Console.WriteLine("Non-Parallel - User: {0} | Index: {1} | Thread: {2}", user.id.name + user.id.value, i, Thread.CurrentThread.ManagedThreadId);
                }
                Console.WriteLine("        Time: {0}", sw.Elapsed);
                return nonparallellist;
            }
        }



        public User InitUser(bool fromWeb)
        {
            Random rand = new Random();
            if (fromWeb)
            {
                string json;

                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://randomuser.me/api/");
                }

                RootObject rootobject = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                User user = rootobject.results[0];
                user.location.postcode = IntExtension.ToInt32OrDefault(user.location.postcode.ToString(), 0);
                return user;
            }
            else
            {
                string json = "{'results': ['gender': 'male','name': {'title': 'mr','first': 'jimi','last': 'nurmi'},'location': {'street': '3091 hermiankatu','city': 'toholampi','state': 'finland proper','postcode': 11638},'email': 'jimi.nurmi@example.com','login': {'username': 'yellowfish277','password': 'skyline','salt': 'OBc7C3HU','md5': 'cc82e791d7698b921dd341faa0fabef8','sha1': 'fa21c2d2f995b58e78501708a07805ad0d9943e2','sha256': '51d3e0ad58f39ec75453845450b4d930ec84a654749f1f1cd9e550ac14d0d098'},'dob': '1979-03-26 14:22:38','registered': '2003-03-16 07:10:23','phone': '07-631-835','cell': '043-317-77-43','id': {'name': 'HETU','value': '379-8219'},'picture': {'large': 'https://randomuser.me/api/portraits/men/37.jpg','medium': 'https://randomuser.me/api/portraits/med/men/37.jpg','thumbnail': 'https://randomuser.me/api/portraits/thumb/men/37.jpg'},'nat': 'FI'}],'info': {'seed': '6e62eb2a6662c7ce','results': 1,'page': 1,'version': '1.1'}}";
                RootObject rootobject = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                User user = rootobject.results[0];
                user.location.postcode = IntExtension.ToInt32OrDefault(user.location.postcode.ToString(), 0);
                return user;
            }
        }

    }


}

