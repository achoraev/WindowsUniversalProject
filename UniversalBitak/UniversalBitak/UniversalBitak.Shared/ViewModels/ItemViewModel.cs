namespace UniversalBitak.ViewModels
{
    using GalaSoft.MvvmLight;
    using Parse;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Text;
    using UniversalBitak.Models;
    using UniversalBitak.Pages;

    //public abstract class ViewModelBase : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        if (this.PropertyChanged != null)
    //        {
    //            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}

    public class ItemViewModel : ViewModelBase
    {
        private string ItemDescription;
        public static Expression<Func<Item, ItemViewModel>> FromModel
        {
            get
            {
                return model => new ItemViewModel()
                {
                    itemName = model.itemName,
                    itemDescription = model.itemDescription,
                    itemPrice = model.itemPrice,
                    itemPicture = model.itemPicture
                };
            }
        }     

        public string itemDescription
        {
            get
            {
                return this.ItemDescription;
            }
            set
            {
                this.ItemDescription = value;
                this.RaisePropertyChanged(() => this.itemDescription);
            }
        }        

        public string itemName { get; set; }

        public double itemPrice { get; set; }

        public Uri itemPicture { get; set; }
    }
}