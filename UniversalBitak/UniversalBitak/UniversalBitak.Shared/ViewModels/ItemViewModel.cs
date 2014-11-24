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
                    itemCategory = model.itemCategory,
                    itemPrice = model.itemPrice,
                    itemPicture = model.itemPicture,
                    user = model.user
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

        public ParseFile itemPicture { get; set; }

        public string itemCategory { get; set; }

        public string user { get; set; }
    }
}