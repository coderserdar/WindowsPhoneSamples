using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;

namespace CustomerManager
{
    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ID { get; set; }

        public Customer(string inName, string inAddress, int inID)
        {
            Name = inName;
            Address = inAddress;
            ID = inID;
        }
    }

    public class Customers
    {
        public string Name { get; set; }

        public Customers(string inName)
        {
            Name = inName;
            CustomerList = new List<Customer>();
        }

        public List<Customer> CustomerList;

        public static Customers MakeTestCustomers()
        {
            string[] firstNames = new string[] { "Rob", "Jim", "Joe", "Nigel", "Sally", "Tim" };
            string[] lastsNames = new string[] { "Smith", "Jones", "Bloggs", "Miles", "Wilkinson", "Brown" };
            Customers result = new Customers("Test Customers");
            int id = 0;

            foreach (string lastName in lastsNames)
            {
                foreach (string firstname in firstNames)
                {
                    //Construct a customer name
                    string name = firstname + " " + lastName;
                    //Add the new customer to the list
                    result.CustomerList.Add(new Customer(name, name + "'s House", id));
                    // Increase the ID for the next customer
                    id++;
                }
            }
            return result;
        }
    }
}
