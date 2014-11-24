namespace UniversalBitak.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using SQLite;

    [Table("ItemForSql")]
    public class ItemForSql
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Unique, MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public double Price { get; set; }
    }
}