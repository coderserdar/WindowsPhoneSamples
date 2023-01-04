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

namespace AddingMachine 
{
    public class AdderClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int topValue;

        public int TopValue
        {
            get
            {
                return topValue;
            }
            set
            {
                topValue = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AnswerValue"));
                }
            }
        }

        // repeat for bottom value

        private int bottomValue;

        public int BottomValue
        {
            get
            {
                return bottomValue;
            }
            set
            {
                bottomValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AnswerValue"));
                }
            }
        }

        private int answerValue;

        public int AnswerValue
        {
            get
            {
                return topValue + bottomValue;
            }
        }
    }
}
