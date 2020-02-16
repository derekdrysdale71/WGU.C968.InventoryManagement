using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WGU.C968.InventoryManagement.Domain
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int InStock { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public ObservableCollection<Part> AssociatedParts = new ObservableCollection<Part>();

        public Product()
        {

        }


        public void AddAssociatedPart(Part part)
        {
            AssociatedParts.Add(part);
        }

        public bool RemoveAssociatedPart(int partId)
        {
            var partToRemove = AssociatedParts.Where(p => p.PartId == partId).FirstOrDefault();
            if (partToRemove == null)
                throw new Exception(message: $"A part with ID #{partId} is not associated with this product.");

            AssociatedParts.Remove(partToRemove);
            return true;
        }

        public Part LookupAssociatedPart(int partId)
        {
            var part = AssociatedParts.Where(p => p.PartId == partId).FirstOrDefault();

            if (part == null)
                throw new Exception(message: $"A part with ID #{partId} is not associated with this product.");

            return part;
        }
    }
}
