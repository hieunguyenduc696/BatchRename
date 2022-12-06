using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class Folder : INotifyPropertyChanged
    {
        public string Foldername
        {
            get; set;
        }

        public string Newfolder
        {
            get; set;
        }

        public string Path
        {
            get; set;
        }

        public string Erro
        {
            get; set;
        }

        public Folder()
        {
            
        }

        public Folder Clone()
        {
            return new Folder()
            {
                Foldername = this.Foldername,
                Newfolder = this.Newfolder,
                Path = this.Path,
                Erro = this.Erro
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
