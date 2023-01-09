using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class PageInfo : INotifyPropertyChanged
    {
       public int Total { get; set; }
        public int PerPage { get; set; }
        public int CurPage { get; set; }
        public int TotalPages { get; set; }
        public bool FilterOn { get; set; }

        public PageInfo()
        {
            Total = 0;
            PerPage = 1;
            CurPage = 1;
            TotalPages = 1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
