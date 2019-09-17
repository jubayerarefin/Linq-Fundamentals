using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> Square = x => x * x;
            //Alternate local function
            //int Square(int x) => x * x;
            Console.WriteLine($"Square of 5 is: {Square(5)}");

            Func<int, int, int> Add = (x, y) => x + y;
            Console.WriteLine($"SUM of 5 & 7 is: {Add(5, 7)}");

            Action<int> write = x => Console.WriteLine(x);
            //Alternate local function
            //void write(int x) => Console.WriteLine(x);
            write(Square(Add(4, 4)));

            //Array of developers of Employee Type
            Employee[] developers = new Employee[]
            {
                new Employee { Id=1,Name="Scott"},
                new Employee { Id=2,Name="Chris"}
            };

            //Array of sales persons of Employee Type
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee { Id=3,Name="Alex"}
            };

            Console.WriteLine($"Total number of developers is: {developers.Count()}");

            var query_1 = developers.Where(e => e.Name.Length == 5)
                                    .OrderBy(e => e.Name);

            var query_2 = from developer in developers
                          where developer.Name.Length == 5
                          orderby developer.Name
                          select developer;

            foreach (var employee in query_1)
            {
                Console.WriteLine(employee.Name);
            }

            Console.WriteLine("\n***\n");

            foreach (var employee in query_2)
            {
                Console.WriteLine(employee.Name);
            }
        }
    }
}
