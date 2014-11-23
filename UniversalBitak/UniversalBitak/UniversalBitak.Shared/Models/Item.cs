﻿namespace UniversalBitak.Models
{
    using Parse;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [ParseClassName("Item")]
    public class Item: ParseObject
    {
        [ParseFieldName("itemName")]
        public string itemName 
        { 
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("itemDescription")]
        public string itemDescription
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("itemCategory")]
        public string itemCategory
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
        [ParseFieldName("user")]
        public string user
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }
        [ParseFieldName("url")]
        public Uri url
        {
            get { return GetProperty<Uri>(); }
            set { SetProperty<Uri>(value); }
        }

        [ParseFieldName("itemPrice")]
        public double itemPrice
        {
            get { return GetProperty<double>(); }
            set { SetProperty<double>(value); }
        }
    }
}