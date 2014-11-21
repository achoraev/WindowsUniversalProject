namespace UniversalBitak.Models
{
    using Parse;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [ParseClassName("Item")]
    public class Item: ParseObject
    {
        public string itemName { get; set; }

        public string itemDescription { get; set; }

        public string itemCategory { get; set; }

        public string user { get; set; }

        public Uri url { get; set; }

        //public PFFile itemPicture { get; set; }

        public decimal itemPrice { get; set; } 
    }
}