using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcesCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");
            //Query syntax
            var query = from car in cars
                        join manufacturer in manufacturers
                            on car.Manufacturer equals manufacturer.Name
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquarters,
                            car.Name,
                            car.Combined
                        };
            //Extension method syntax with Select projection
            var query_3 = cars.Join(manufacturers,
                                    c => c.Manufacturer,
                                    m => m.Name, (c, m) => new
                                    {
                                        m.Headquarters,
                                        c.Name,
                                        c.Combined
                                    })
                               .OrderByDescending(c => c.Combined)
                               .ThenBy(c => c.Name);

            Console.Write("**Car Summary Begins**\n");
            //foreach (var car in query)
            foreach (var car in query_3)
            {
                Console.WriteLine($"{car.Headquarters,-7} : {car.Name,-32} : {car.Combined}");
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //var query_1 = cars.OrderByDescending(c => c.Combined).ThenBy(c => c.Name);
            var query_1 = from car in cars
                          where car.Manufacturer == "BMW" && car.Year == 2016
                          orderby car.Combined descending, car.Name ascending
                          select new
                          {
                              car.Manufacturer,
                              car.Name,
                              car.Combined
                          };
            var query_2 = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                              .OrderByDescending(c => c.Combined)
                              .ThenBy(c => c.Name)
                              .Select(c => c);
            var top = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                              .OrderByDescending(c => c.Combined)
                              .ThenBy(c => c.Name)
                              .Select(c => c)
                              .First();
            Console.WriteLine($"{top.Name} : {top.Combined}");

            var result_1 = cars.Any(c => c.Manufacturer == "Ford");

            //var result_2 = cars.SelectMany(c => c.Name);

            //foreach (var character in result_2)
            //{
            //    Console.WriteLine(character);
            //}

            Console.WriteLine($"{result_1}");

            foreach (var car in query_2)
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(",");
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = int.Parse(columns[2]),
                    };
                });
            return query.ToList();
        }

        private static List<Car> ProcesCars(string path)
        {
            var query = File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 1)
                .ToCar();
            return query.ToList();
            /*
            File.ReadAllLines(path)
            .Skip(1)
            .Where(line => line.Length > 1)
            .Select(Car.ParseFromCSV)
            .ToList();
            */
        }
    }
    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(",");

                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }
        }
    }
}
