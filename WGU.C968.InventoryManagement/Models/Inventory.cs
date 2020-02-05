using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WGU.C968.InventoryManagement.Models
{
    public class Inventory
    {
        public static BindingList<Product> Products = new BindingList<Product>();
        public static BindingList<Part> Parts = new BindingList<Part>();

        public Inventory(bool seedInitialInventory = true)
        {
            if (seedInitialInventory)
            {
                SeedInventory();
            }
        }


        // Product Actions
        public void AddProduct(Product product)
        {
            // TODO: Validate product first?
            Products.Add(product);
        }

        public bool RemoveProduct(int productId)
        {
            var productToRemove = Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (productToRemove == null) return false;

            // TODO: Check for associated parts

            Products.Remove(productToRemove);
            return true;
        }

        public Product LookupProduct(int productId)
        {
            var product = Products.Where(p => p.ProductId == productId).FirstOrDefault();

            if (product == null) 
                throw new Exception(message: $"A product with ID #{productId} could not be found.");

            return product;
        }

        public void UpdateProduct(int productId, Product product)
        {
            var productToUpdate = Products.Where(p => p.ProductId == productId).FirstOrDefault();

            if (productToUpdate == null)
                throw new Exception(message: $"A product with ID #{productId} could not be found.");

            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.InStock = product.InStock;
            productToUpdate.Min = product.Min;
            productToUpdate.Max = product.Max;
        }


        // Part Actions
        public void AddPart(Part part)
        {
            // TODO: Validate part first?
            Parts.Add(part);
        }

        public bool DeletePart(Part part)
        {
            var partToDelete = Parts.Where(p => p.PartId == part.PartId).FirstOrDefault();
            if (partToDelete == null) return false;

            Parts.Remove(part);
            return true;
        }

        public Part LookupPart(int partId)
        {
            var part = Parts.Where(p => p.PartId == partId).FirstOrDefault();

            if (part == null)
                throw new Exception(message: $"A part with ID #{partId} could not be found.");

            return part;
        }

        public void UpdatePart(int partId, Part part)
        {
            var partToUpdate = Parts.Where(p => p.PartId == partId).FirstOrDefault();

            if (partToUpdate == null)
                throw new Exception(message: $"A part with ID #{partId} could not be found.");

            partToUpdate.Name = part.Name;
            partToUpdate.Price = part.Price;
            partToUpdate.InStock = part.InStock;
            partToUpdate.Min = part.Min;
            partToUpdate.Max = part.Max;
        }

        private void SeedInventory()
        {
            Parts.Add(new InhousePart
            {
                PartId = 1,
                Name = "Whatchamacallit",
                Price = 3.01m,
                InStock = true,
                Min = 1,
                Max = 10,
                MachineId = 1
            });

            Parts.Add(new OutsourcedPart
            {
                PartId = 2,
                Name = "Thingamajigger",
                Price = 7.52m,
                InStock = true,
                Min = 1,
                Max = 20,
                CompanyName = "ACME Part Co."
            });

            Parts.Add(new OutsourcedPart
            {
                PartId = 3,
                Name = "Doohickey",
                Price = 4.07m,
                InStock = true,
                Min = 1,
                Max = 20,
                CompanyName = "ACME Part Co."
            });
        }
    }
}
