namespace UniversalBitak.ViewModels
{    
    using System;
    using System.Collections.Generic;
    using System.Text;
    using GalaSoft.MvvmLight;

    public class ItemDetailsPageViewModel: ViewModelBase
    {
        private ItemViewModel itemVM;
        public ItemViewModel Item
        {
            get
            {
                return this.itemVM;
            }
            set
            {
                this.itemVM = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }
    }
}