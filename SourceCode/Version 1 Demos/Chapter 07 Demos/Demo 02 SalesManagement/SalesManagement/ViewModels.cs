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

using System.ComponentModel;

namespace SalesManagement
{

    public class CustomerView : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private string address;

        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
                }
            }
        }

        private string bankDetails;

        public string BankDetails
        {
            get
            {
                return bankDetails;
            }
            set
            {
                bankDetails = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BankDetails"));
                }
            }
        }

        private int id;

        public int ID
        {
            get
            {
                return id;
            }
        }

        public void Load(Customer cust)
        {
            Name = cust.Name;
            Address = cust.Address;
            BankDetails = cust.BankDetails;
            id = cust.CustomerID;
        }

        public void Save(Customer cust)
        {
            cust.Name = Name;
            cust.Address = Address;
            cust.BankDetails = BankDetails;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ProductView : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private string supplier;

        public string Supplier
        {
            get
            {
                return supplier;
            }
            set
            {
                supplier = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Supplier"));
                }
            }
        }

        private int price;

        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Price"));
                }
            }
        }

        private int id;

        public int ID
        {
            get
            {
                return id;
            }
        }

        public void Load(Product product)
        {
            Name = product.Name;
            Supplier = product.Supplier;
            Price = product.Price;
            id = product.ProductID;
        }

        public void Save(Product product)
        {
            product.Name = Name;
            product.Supplier = Supplier;
            product.Price = Price;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
