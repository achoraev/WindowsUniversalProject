namespace UniversalBitak.ViewModels
{
    using Parse;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using UniversalBitak.Models;
    using UniversalBitak.Pages;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ItemViewModel : ViewModelBase
    {
        private const string DefaultViewModelName = "No name";

        public List<Item> ItemsOfItem;        

        public string itemName { get; set; }

        public string itemCategory { get; set; }

        public string itemDescription { get; set; }

        public Uri url { get; set; }

        // public decimal itemPrice { get; set; }

        public ItemViewModel() :
            this(DefaultViewModelName)
        {
        }

        public ItemViewModel(string itemName)
        {
            this.itemName = itemName;
            //this.itemDescription = itemDescription;               
        }        
    }
}