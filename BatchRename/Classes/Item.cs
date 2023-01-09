using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class Item
    {
        public string Name
        {
            get; set;
        }

        public string NewName
        {
            get; set;
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

        public Item Clone()
        {
            return new Item()
            {
                Name = this.Name,
                NewName = this.NewName,
                Path = this.Path,
                Erro = this.Erro
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}