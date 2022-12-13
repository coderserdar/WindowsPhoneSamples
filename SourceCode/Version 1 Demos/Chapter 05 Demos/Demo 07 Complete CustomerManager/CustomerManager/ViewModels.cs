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

namespace CustomerManager
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
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
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
                    PropertyChanged(this, new PropertyChangedEventArgs("address"));
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
            id = cust.ID;
        }

        public void Save(Customer cust)
        {
            cust.Name = Name;
            cust.Address = Address;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
