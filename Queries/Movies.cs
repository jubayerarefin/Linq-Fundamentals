using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
    class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }

        int _year;
        public int Year
        {
            get
            {
                Console.WriteLine($"Returning {_year} of the {Title}");
                return _year;
            }
            set => _year = value;
        }


        public new string ToString
        {
            get
            {
                return $"{Title,-20} {Rating,-5} {Year}";
            }
        }

    }
}
