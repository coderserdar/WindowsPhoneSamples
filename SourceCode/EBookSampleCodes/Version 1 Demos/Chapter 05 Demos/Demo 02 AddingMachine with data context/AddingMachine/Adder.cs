using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddingMachine
{
    public class Adder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int v1Value;

        public int V1
        {
            get
            {
                return v1Value;
            }
            set
            {
                v1Value = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("Answer"));
                }
            }
        }

        void a_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private int v2Value;

        public int V2
        {
            get
            {
                return v2Value;
            }
            set
            {
                v2Value = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("Answer"));
                }
            }
        }

        public int Answer
        {
            get
            {
                return v1Value + v2Value;
            }
        }
    }
}
