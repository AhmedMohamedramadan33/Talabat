using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Order_Aggregate
{
    public class Address
    {
        public Address(string firstName, string lName, string street, string city, string country)
        {
            FirstName = firstName;
            LastName = lName;
            Street = street;
            City = city;
            Country = country;
        }
        public Address()
        {

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
