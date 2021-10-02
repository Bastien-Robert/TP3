using System;
using System.Linq;
using System.Threading;

namespace TP3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var collection = new MovieCollection().Movies;
            
            Console.WriteLine("----Count all movies :");
            Console.WriteLine(collection.Count);
            
            Console.WriteLine("----Count all movies with the letter e :");
            Console.WriteLine(collection.Count(x => x.Title.Contains('e')));
            
            Console.WriteLine("----Count how many time the letter f is in all the titles from this list :");
            var counterF = 0;
            foreach (var x in collection)
            {
                counterF += x.Title.Count(x => x == 'f');
            }
            Console.WriteLine(counterF);

            Console.WriteLine("----Display the title of the film with the higher budget :");
            var highBudget = from x in collection orderby x.Budget descending select x.Title;
            Console.WriteLine(highBudget.First());
            
            Console.WriteLine("----Display the title of the movie with the lowest box office :");
            var lowestBoxOffice = from x in collection orderby x.BoxOffice ascending select x.Title;
            Console.WriteLine(lowestBoxOffice.First());

            Console.WriteLine("----Order the movies by reversed alphabetical order and print the first 11 of the list :");
            var reversedOrder = from x in collection orderby x.Title descending select x.Title;
            foreach (var x in reversedOrder.Take(11))
            {
                Console.WriteLine(x);
            }
            
            Console.WriteLine("----Count all the movies made before 1980 :");
            var before1980 = from x in collection where x.ReleaseDate.Year <1980 select x;
            Console.WriteLine(before1980.Count());
            
            Console.WriteLine("----Display the average running time of movies having a vowel as the first letter :");
            Console.WriteLine((from x in collection where "aeiou".IndexOf(x.Title.ToLower()[0]) >= 0 select x.RunningTime).Average());
            
            Console.WriteLine("----Calculate the mean of all Budget / Box Office of every movie ever :");
            Console.WriteLine($"Average Budget : {(from x in collection select x.Budget).Average()}");
            Console.WriteLine($"Average Box Office : {(from x in collection select x.BoxOffice).Average()}");
            Console.WriteLine($"Average Budget / Average Box Office : {(from x in collection select x.Budget).Average()/(from x in collection select x.BoxOffice).Average()}");
            
            Console.WriteLine("----Print all movies with the letter H or W in the title, but not the letter I or T :");
            foreach (var movie in from x in collection where (x.Title.ToUpper().Contains('H') || x.Title.ToUpper().Contains('W')) && !(x.Title.ToUpper().Contains('I') || x.Title.ToUpper().Contains('T')) select x) 
            {
                Console.WriteLine(movie.Title);
            }
            
            Console.WriteLine("----Exo 2 :");
            CreateThread();
        }
        public static void CreateThread()
        {
            Thread t1 = new Thread(new ThreadStart(Thread1));
            Thread t2 = new Thread(new ThreadStart(Thread2));
            Thread t3 = new Thread(new ThreadStart(Thread3));
            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("End of threads");
        }

        public static void Thread1()
        {
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(10))
            {
                Print('_');
                Thread.Sleep(50);
            }
        }

        public static void Thread2()
        {
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(11))
            {
                Print('*');
                Thread.Sleep(40);
            }
        }

        public static void Thread3()
        {
            var startTime = DateTime.UtcNow;
            while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(9))
            {
                Print('°');
                Thread.Sleep(20);
            }
        }


        private static readonly Mutex m = new Mutex();

        public static void Print(char c)
        {
            m.WaitOne();
            try
            {
                Console.Write(c);
            }
            finally
            {
                m.ReleaseMutex();
            }
        }
    }
}