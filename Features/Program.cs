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
            //Alternate local methid
            //int Square(int x) => x * x;
            Console.WriteLine($"Square of 5 is: {Square(5)}");

            Func<int, int, int> Add = (x, y) => x + y;
            Console.WriteLine($"SUM of 5 & 7 is: {Add(5, 7)}");

            Action<int> write = x => Console.WriteLine(x);
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

            foreach (var employee in developers.Where(e => e.Name.Length == 5)
                                               .OrderBy(e => e.Name))
            {
                Console.WriteLine(employee.Name);
            }
        }
    }
}
