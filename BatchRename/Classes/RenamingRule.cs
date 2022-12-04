using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class RenamingRule : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public bool Checked { get; set; }

        public RenamingRule()
        {
        }
        public RenamingRule(string name, bool isChecked)
        {
            Name = name;
            Checked = isChecked;
        }

        public RenamingRule Clone()
        {
            return new RenamingRule(Name, Checked);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
