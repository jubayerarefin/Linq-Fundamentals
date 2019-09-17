using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }

            var movies = new List<Movie>
            {
                new Movie{Title="The Dark Knight", Rating=8.9f,Year=2008},
                new Movie{Title="The Kings Speech", Rating=8.0f,Year=2010},
                new Movie{Title="Casablanca", Rating=8.5f,Year=1942},
                new Movie{Title="Star Wars V", Rating=8.7f,Year=1980},
            };

            var query_1 = Enumerable.Empty<Movie>();
            query_1 = movies.Where(m => m.Year > 2000)
                            /**
                             * OrderBy is a Non streaming operator where Order is a streaming operator
                             * So it is better to filter first then sort
                             */
                            .OrderByDescending(m => m.Rating);

            //Later
            query_1 = query_1.Take(1);
            foreach (var movie in query_1)
            {
                Console.WriteLine(movie.Title);
            }

            //Custom Filter
            //var query_2 = movies.Filter(m => m.Year > 2000);

            //Forces to execute immediately rather than deferred execution
            //query_2.Count();

            //var enumerator = query_2.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current.Title);
            //}
        }
    }
}
