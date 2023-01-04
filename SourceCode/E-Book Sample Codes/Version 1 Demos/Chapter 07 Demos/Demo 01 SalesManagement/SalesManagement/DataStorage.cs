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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SalesManagement
{
    [Table]
    public class Customer : INotifyPropertyChanged,
                        INotifyPropertyChanging
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int CustomerID { get; set; }

        private string nameValue;
        [Column]
        public string Name
        {
            get
            {
                return nameValue;
            }
            set
            {
                NotifyPropertyChanging("Name");
                nameValue = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string addressValue;
        [Column]
        public string Address
        {
            get
            {
                return addressValue;
            }
            set
            {
                NotifyPropertyChanging("Address");
                addressValue = value;
                NotifyPropertyChanged("Address");
            }
        }

        private string BankDetailsValue;
        [Column]
        public string BankDetails
        {
            get
            {
                return BankDetailsValue;
            }
            set
            {
                NotifyPropertyChanging("BankDetails");
                BankDetailsValue = value;
                NotifyPropertyChanged("BankDetails");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                              new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this,
                              new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
    }

    public class SalesDB : DataContext
    {
        public string Name { get; set; }

        public Table<Customer> CustomerTable;

        public SalesDB(string connection)
            : base(connection)
        {
        }

        public static void MakeTestDB(string connection)
        {
            string[] firstNames = new string[] { "Rob", "Jim", "Joe", "Nigel", "Sally", "Tim" };
            string[] lastsNames = new string[] { "Smith", "Jones", "Bloggs", "Miles", "Wilkinson", "Brown" };

            SalesDB newDB = new SalesDB(connection);

            if (newDB.DatabaseExists())
            {
                newDB.DeleteDatabase();
            }

            newDB.CreateDatabase();

            foreach (string lastName in lastsNames)
            {
                foreach (string firstname in firstNames)
                {
                    //Construct some customer details
                    string name = firstname + " " + lastName;
                    string address = name + "'s address";
                    string bank = name + "'s bank";
                    Customer newCustomer = new Customer();
                    newCustomer.Name = name;
                    newCustomer.Address = address;
                    newCustomer.BankDetails = bank;
                    newDB.CustomerTable.InsertOnSubmit(newCustomer);
                }
            }
            newDB.SubmitChanges();
        }
    }
}
