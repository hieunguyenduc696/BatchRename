using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class Parameter : INotifyPropertyChanged, ICloneable
    {
        public string Name
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }

        public string StringValue
        {
            get; set;
        }

        public void copyContent(string Name, string Type, string StringValue)
        {
            this.Name = Name;
            this.Type = Type;
            this.StringValue = StringValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
