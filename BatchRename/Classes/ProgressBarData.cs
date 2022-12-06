using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class ProgressBarData : INotifyPropertyChanged, ICloneable
    {
        public int Total { get; set; }
        public int Current { get; set; }
       
        public string CurrentName { get; set; }

        public void setNewProgress(int Current, string CurrentName, int Total)
        {
            this.Current = Current;
            this.CurrentName = CurrentName;
            this.Total = Total;
        }
        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
