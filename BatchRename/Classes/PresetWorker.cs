using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchRename
{
    public class PresetWorker : INotifyPropertyChanged, ICloneable
    {
        public string Name { get; set; }

        public Dictionary<string, bool> checkedList { get; set; }
        public BindingList<RenamingRule> PresetRules { get; set; }

        public PresetWorker()
        {
            this.checkedList = new Dictionary<string, bool>();
        }

        public string ToString()
        {
            return this.Name;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void ParseConfigToFile(string location)
        {
            string configContent = "";
            for (int i = 0; i < PresetRules.Count; i++)
            {
                if (i != PresetRules.Count - 1)
                {
                    configContent += PresetRules[i].StringParameters + "\n";
                }
                else
                {
                    configContent += PresetRules[i].StringParameters;
                }
            }
            try 
            {
                using (StreamWriter sw = System.IO.File.CreateText(location))
                {
                    sw.Write(configContent);
                }
                System.Windows.MessageBox.Show("Done saving preset", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Failed to save preset", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
