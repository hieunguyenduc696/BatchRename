using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BatchRename
{
    public class ViewModel : INotifyPropertyChanged
    {
        public List<Item> items { get; set; }
        public List<Item> selectedItems { get; set; }
        public int Total { get; set; }
        public int PerPage { get; set; }
        public int CurPage { get; set; }
        public int TotalPages { get; set; }

        public string Type { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void changeItemPage(int pageNumber)
        {
            selectedItems = items.Skip((pageNumber - 1) * PerPage).Take(PerPage).ToList();
            CurPage = pageNumber;
        }

        public List<Item> updatePerPage(int newPerPage)
        {
            this.PerPage = newPerPage;
            this.TotalPages = Total / PerPage + ((Total % PerPage) == 0 ? 0 : 1);
            if (CurPage > TotalPages)
            {
                CurPage = TotalPages;
            }
            selectedItems = items.Skip((CurPage - 1) * PerPage).Take(PerPage).ToList();
            return selectedItems;
        }
        public void updateItemPage(Item newItem, int idx)
        {
            int sourceIdx = idx + PerPage * (CurPage - 1);
            items[sourceIdx] = ((Item)newItem).Clone();
            selectedItems[idx] = ((Item)newItem).Clone();
        }

        public void removeItemPage(int idx)
        {
            int sourceIdx = idx + PerPage * (CurPage - 1);
            items.RemoveAt(sourceIdx);
            Total = items.Count;
            TotalPages = Total / PerPage + ((Total % PerPage) == 0 ? 0 : 1);
           
            if (CurPage > TotalPages)
            {
                CurPage = TotalPages;
            }
            selectedItems = items.Skip((CurPage - 1) * PerPage).Take(PerPage).ToList();

        }


        public void addItemPage(Item newItem)
        {
            items.Add(newItem);

            Total = items.Count;
            TotalPages = Total / PerPage + ((Total % PerPage) == 0 ? 0 : 1);
            selectedItems = items.Skip((CurPage - 1) * PerPage).Take(PerPage).ToList();
        }

        public void Setup(List<Item> items, int curPage, int perPage)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = (Item)items[i];
            }

            Total = items.Count;
            PerPage = perPage;
            CurPage = curPage;
            TotalPages = Total / PerPage + ((Total % PerPage) == 0 ? 0 : 1);
            selectedItems = items.Skip((CurPage - 1) * PerPage).Take(PerPage).ToList();
        }


        public ViewModel()
        {
            items = new List<Item>();
            selectedItems = new List<Item>();

            Total = 0;
            PerPage = CurPage = TotalPages = 1;
        }

        public ViewModel(string viewType)
        {
            items = new List<Item>();
            selectedItems = new List<Item>();

            Total = 0;
            PerPage = CurPage = TotalPages = 1;

            Type = viewType;
        }

    }
}
