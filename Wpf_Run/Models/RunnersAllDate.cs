using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Run.Models
{
    class RunnersAllData : INotifyPropertyChanged
    {
        public string User { get; set; }
        public double average_staps { get; set; }
        public int maximum_staps { get; set; }
        public int minimum_Staps { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChabged(string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
