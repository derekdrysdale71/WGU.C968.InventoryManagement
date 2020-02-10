using System;
using System.Collections.Generic;
using System.Text;

namespace WGU.C968.InventoryManagement.Domain
{
    public abstract class Part
    {
        public int PartId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }
    }
}
