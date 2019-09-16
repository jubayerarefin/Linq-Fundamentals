using System;
using System.Collections.Generic;
using System.Linq;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.WriteLine(developers.Count());

            foreach (var employee in developers.Where(e => e.Name.StartsWith('S')))
            {
                Console.WriteLine(employee.Name);
            }

        }
    }
}
