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
            var query_5 = from car in cars
                          group car
                          by car.Manufacturer.ToUpper() into manufacturer
                          orderby manufacturer.Key
                          select manufacturer;

            Console.Write("**Query_5**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_5)
            {
                Console.WriteLine(group.Key);

                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Extension method syntax
            var query_6 = cars.GroupBy(c => c.Manufacturer.ToUpper())
                              .OrderBy(g => g.Key);

            Console.Write("**Query_6**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_6)
            {
                Console.WriteLine(group.Key);

                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Query method syntax
            var query_7 = from manufacturer in manufacturers
                          join car in cars on manufacturer.Name equals car.Manufacturer
                          into carGroup
                          orderby manufacturer.Name
                          select new
                          {
                              Manufacturer = manufacturer,
                              Cars = carGroup
                          };

            Console.Write("**Query_7**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_7)
            {
                Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");

                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Extension method syntax
            var query_8 = manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer,
                (m, g) => new
                {
                    Manufacturer = m,
                    Cars = g
                })
                .OrderBy(m => m.Manufacturer.Name);

            Console.Write("**Query_8**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_8)
            {
                Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");

                foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Query method syntax
            var query_9 = from car in cars
                          group car by car.Manufacturer into carGroup
                          select new
                          {
                              Name = carGroup.Key,
                              Max = carGroup.Max(c => c.Combined),
                              Min = carGroup.Min(c => c.Combined),
                              Avg = carGroup.Average(c => c.Combined)
                          } into result
                          orderby result.Max descending
                          select result;

            Console.Write("**Query_9**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_9)
            {
                Console.WriteLine($"{group.Name}");
                Console.WriteLine($"\t{group.Max}");
                Console.WriteLine($"\t{group.Min}");
                Console.WriteLine($"\t{group.Avg}");

            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Extension method syntax for Aggregration with Accumulator
            var query_10 = cars.GroupBy(c => c.Manufacturer)
                .Select(g =>
                {
                    var results = g.Aggregate(new CarStatistics(), (acc, c) => acc.Accumulate(c),
                        acc => acc.Compute());

                    return new
                    {
                        Name = g.Key,
                        Avg = results.Average,
                        results.Min,
                        results.Max
                    };
                })
                .OrderByDescending(r => r.Max);

            Console.Write("**Query_10**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var group in query_10)
            {
                Console.WriteLine($"{group.Name}");
                Console.WriteLine($"\t{group.Max}");
                Console.WriteLine($"\t{group.Min}");
                Console.WriteLine($"\t{group.Avg}");
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Query syntax
            var query = from car in cars
                        join manufacturer in manufacturers
                            on new { car.Manufacturer, car.Year } equals new { Manufacturer = manufacturer.Name, manufacturer.Year }
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            manufacturer.Headquarters,
                            car.Name,
                            car.Combined
                        };
            Console.Write("**Query**\n");
            Console.Write("**Car Summary Begins**\n");
            foreach (var car in query)
            {
                Console.WriteLine($"{car.Headquarters,-7} : {car.Name,-32} : {car.Combined}");
            }
            Console.WriteLine("\n**Car Summary Ends**\n");

            //Extension method syntax with inline projection within Join insteadof Select
            var query_3 = cars.Join(manufacturers,
                                    c => new { c.Manufacturer, c.Year },
                                    m => new { Manufacturer = m.Name, m.Year },
                                    (c, m) => new
                                    {
                                        m.Headquarters,
                                        c.Name,
                                        c.Combined
                                    })
                               .OrderByDescending(c => c.Combined)
                               .ThenBy(c => c.Name);

            Console.Write("**Query_3**\n");
            Console.Write("**Car Summary Begins**\n");
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
    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }

        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Min(Min, car.Combined);

            return this;
        }

        public CarStatistics Compute()
        {
            Average = Total / Count;

            return this;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public double Average { get; set; }
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
