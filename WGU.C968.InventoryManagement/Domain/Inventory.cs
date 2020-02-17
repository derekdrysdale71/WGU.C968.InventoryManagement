using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WGU.C968.InventoryManagement.Domain
{
    public class Inventory : INotifyPropertyChanged
    {
        private List<Part> _parts = new List<Part>();
        private List<Product> _products = new List<Product>();

        public IEnumerable<Part> Parts => _parts.Where(p => p.Name.Contains(PartSearchTerm ?? "", StringComparison.CurrentCultureIgnoreCase));
        public IEnumerable<Product> Products => _products.Where(p => p.Name.Contains(ProductSearchTerm ?? "", StringComparison.CurrentCultureIgnoreCase));

        public string PartSearchTerm { get; set; }
        public string ProductSearchTerm { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
            _products.Add(product);
        }

        public bool RemoveProduct(int productId)
        {
            var productToRemove = Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (productToRemove == null) 
                throw new Exception($"A product with ID #{productId} could not be found.");

            // Product cannot be deleted if it has parts associated with it.
            if (productToRemove.AssociatedParts.Any())
                throw new Exception("Product has associated parts and cannot be deleted.  Please remove parts first.");
            
            _products.Remove(productToRemove);
            return true;
        }

        public Product LookupProduct(int productId)
        {
            return _products.Where(p => p.ProductId == productId).FirstOrDefault();
        }

        public void UpdateProduct(int productId, Product product)
        {
            var productToUpdate = _products.Where(p => p.ProductId == productId).FirstOrDefault();

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
            _parts.Add(part);
        }

        public bool DeletePart(Part part)
        {
            var partToDelete = Parts.Where(p => p.PartId == part.PartId).FirstOrDefault();
            if (partToDelete == null) return false;

            _parts.Remove(part);
            return true;
        }

        public Part LookupPart(int partId)
        {
            return _parts.Where(p => p.PartId == partId).FirstOrDefault();
        }

        public void UpdatePart(int partId, Part part)
        {
            var partToUpdate = _parts.Where(p => p.PartId == partId).FirstOrDefault();

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
            _parts.Add(new InhousePart
            {
                PartId = 1,
                Name = "Whatchamacallit",
                Price = 3.01m,
                InStock = 4,
                Min = 1,
                Max = 10,
                MachineId = 1
            });

            _parts.Add(new OutsourcedPart
            {
                PartId = 2,
                Name = "Thingamajigger",
                Price = 7.52m,
                InStock = 13,
                Min = 1,
                Max = 20,
                CompanyName = "ACME Part Co."
            });

            _parts.Add(new OutsourcedPart
            {
                PartId = 3,
                Name = "Doohickey",
                Price = 4.07m,
                InStock = 9,
                Min = 1,
                Max = 20,
                CompanyName = "ACME Part Co."
            });

            var associatedParts = new ObservableCollection<Part>();
            associatedParts.Add(LookupPart(1));

            _products.Add(new Product
            {
                ProductId = 1,
                Name = "Widget",
                Price = 15.76m,
                InStock = 506,
                Min = 100,
                Max = 1000,
                AssociatedParts = associatedParts
            });
        } 
    }
}
