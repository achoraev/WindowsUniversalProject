namespace UniversalBitak.ViewModels
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SellerSendViewModel: ViewModelBase
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