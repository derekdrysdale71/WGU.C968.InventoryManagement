using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WGU.C968.InventoryManagement.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public ObservableCollection<Part> AssociatedParts = new ObservableCollection<Part>();
    }
}
