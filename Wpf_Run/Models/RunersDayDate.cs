using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Run.Models
{
    class RunnersDayData :INotifyPropertyChanged
    {
        public int Rank { get; set; }
        public string User { get; set; }
        private bool _ItFinished { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChabged(string name ="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string Status { get;set;}
        public int Steps { get; set; }
    }
}

