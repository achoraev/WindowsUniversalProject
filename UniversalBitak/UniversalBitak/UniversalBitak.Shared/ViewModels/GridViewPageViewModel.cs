namespace UniversalBitak.ViewModels
{
    using Parse;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight;

    using UniversalBitak.Models;

    public class GridViewPageViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> items;
        private bool initializing;

        public GridViewPageViewModel()
        {
            this.LoadItems();
        }

        private async Task LoadItems()
        {
            this.Initializing = true;

            var items = await new ParseQuery<Item>()
            .FindAsync();

            this.Items = items.AsQueryable()
                    .Select(ItemViewModel.FromModel);

            this.Initializing = false;
        }

        public bool Initializing
        {
            get
            {
                return this.initializing;
            }
            set
            {
                this.initializing = value;
                this.RaisePropertyChanged(() => this.Initializing);
            }
        }

        public IEnumerable<ItemViewModel> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new ObservableCollection<ItemViewModel>();
                }
                return this.items;
            }
            set
            {
                if (this.items == null)
                {
                    this.items = new ObservableCollection<ItemViewModel>();
                }
                this.items.Clear();
                foreach (var item in value)
                {
                    this.items.Add(item);
                }
            }
        }
    }
}