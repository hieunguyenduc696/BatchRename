using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class File : INotifyPropertyChanged
    {
        public string Filename
        {
            get; set;
        }

        public string Newfilename
        {
            get;set;
        }

        public string Path
        {
            get;
            set;
           
        }

        public string Erro
        {
            get; set;
        }


        public File Clone()
        {
            return new File()
            {
                Filename = this.Filename,
                Newfilename = this.Newfilename,
                Path = this.Path,
                Erro = this.Erro
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
