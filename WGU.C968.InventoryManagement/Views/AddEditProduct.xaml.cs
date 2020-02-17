using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WGU.C968.InventoryManagement.Domain;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public Part SelectedPart { get; set; }
        public Part SelectedAssociatedPart { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCost
        {
            get { return _productCost; }
            set { _productCost = value; OnPropertyChanged(nameof(ProductCost)); }
        }
        public string ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; OnPropertyChanged(nameof(ProductCount)); }
        }

        public string ProductMin
        {
            get { return _productMin; }
            set { _productMin = value; OnPropertyChanged(nameof(ProductMin)); OnPropertyChanged(nameof(ProductCount)); }
        }

        public string ProductMax
        {
            get { return _productMax; }
            set { _productMax = value; OnPropertyChanged(nameof(ProductMax)); OnPropertyChanged(nameof(ProductCount)); }
        }

        public ObservableCollection<Part> Parts
        {
            get { return _parts; }
            set { _parts = value; OnPropertyChanged(nameof(Parts)); }
        }

        public bool IsNewProduct
        {
            get { return _isNewProduct; }
            set { _isNewProduct = value; OnPropertyChanged(nameof(IsNewProduct)); }
        }

        public bool IsValid
        {
            get
            {
                foreach (var property in ValidatedProperties)
                {
                    if (GetPropertyError(property) != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public ObservableCollection<Part> ProductAssociatedParts { get; set; }

        private readonly MainWindow mainWindow;
        public Inventory CurrentModel { get; set; }
        
        private bool _isNewProduct;
        private string _productCount;
        private string _productCost;
        private string _productMax;
        private string _productMin;
        private ObservableCollection<Part> _parts;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Error => null;
        public string this[string propertyName] => GetPropertyError(propertyName);

        private static readonly string[] ValidatedProperties = { "ProductName", "ProductCost", "ProductCount", "ProductMax", "ProductMin" };

        public AddEditProduct(int productId, Inventory model)
        {
            InitializeComponent();
            this.DataContext = this;

            mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            CurrentModel = model;
            Product = mainWindow.Model.LookupProduct(productId);
            Parts = mainWindow.GetUpdatedParts();

            IsNewProduct = Product == null ? true : false;

            ProductId = Product != null ? Product.ProductId : productId;
            ProductName = Product != null ? Product.Name : "";
            ProductCost = Product != null ? Product.Price.ToString() : "";
            ProductCount = Product != null ? Product.InStock.ToString() : "";
            ProductMin = Product != null ? Product.Min.ToString() : "";
            ProductMax = Product != null ? Product.Max.ToString() : "";
            ProductAssociatedParts = Product != null ? Product.AssociatedParts : new ObservableCollection<Part>();

            if (Product == null)
            {
                Product = new Product();
            }
        }


        private void PartSearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            Parts = mainWindow.GetUpdatedParts();
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPart != null)
            {
                ProductAssociatedParts.Add(SelectedPart);
            }
            else
            {
                MessageBox.Show(
                    "You must select a part to add.",
                    "No Part Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void DeletePartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAssociatedPart != null)
            {
                ProductAssociatedParts.Remove(SelectedAssociatedPart);
            }
            else
            {
                MessageBox.Show(
                    "You must select a part to remove.",
                    "No Part Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid)
            {
                if (IsNewProduct)
                {
                    Product = new Product
                    {
                        ProductId = ProductId,
                        Name = ProductName,
                        Price = decimal.Parse(ProductCost),
                        InStock = int.Parse(ProductCount),
                        Max = int.Parse(ProductMax),
                        Min = int.Parse(ProductMin)
                    };

                    CurrentModel.AddProduct(Product);

                    foreach (var part in ProductAssociatedParts)
                    {
                        Product.AddAssociatedPart(part);
                    }
                }
                else
                {
                    Product.Name = ProductName;
                    Product.Price = decimal.Parse(ProductCost);
                    Product.InStock = int.Parse(ProductCount);
                    Product.Max = int.Parse(ProductMax);
                    Product.Min = int.Parse(ProductMin);
                    Product.AssociatedParts = ProductAssociatedParts;

                    CurrentModel.UpdateProduct(Product.ProductId, Product);
                }

                this.Close();
                mainWindow.GetUpdatedProducts();
                mainWindow.Opacity = 1;
            }
            else
            {
                MessageBox.Show(
                    "You must correct the product errors before saving.",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Opacity = 1;
        }

        private string GetPropertyError(string propertyName)
        {
            int count;
            int min;
            int max;
            
            switch (propertyName)
            {
                case "ProductName":
                    if (string.IsNullOrEmpty(ProductName))
                    {
                        return "Required";
                    }  
                    break;
                case "ProductCount":
                    int.TryParse(ProductMin, out min);
                    int.TryParse(ProductMax, out max);

                    if (string.IsNullOrWhiteSpace(ProductCount))
                    {
                        return "Required";
                    }
                    else if (int.TryParse(ProductCount, out count))
                    {
                        if (min != 0 && max != 0 && min < max && (count < min || count > max))
                        {
                            return $"Must be between {min} and {max}";
                        }
                    }
                    else
                    {
                        return "Must be a number";
                    }
                    break;
                case "ProductCost":
                    if (string.IsNullOrWhiteSpace(ProductCost))
                    {
                        return "Required";
                    }
                    else if (decimal.TryParse(ProductCost, out decimal cost)
                        && cost >= 0
                        && cost * 100 == Math.Floor(cost * 100))
                    {
                        break;
                    }
                    else
                    {
                        return "Must be a price";
                    }
                case "ProductMin":
                    int.TryParse(ProductMin, out min);
                    int.TryParse(ProductMax, out max);

                    if (string.IsNullOrWhiteSpace(ProductMin))
                        return "Required";
                    else if (int.TryParse(ProductMin, out count))
                    {
                        if (count < min || count > max)
                        {
                            return $"Must be less than {ProductMax}";
                        }
                    }
                    else
                    {
                        return "Must be a number";
                    }
                    break;
                case "ProductMax":
                    if (string.IsNullOrWhiteSpace(ProductMax))
                    {
                        return "Required";
                    }
                    else if (int.TryParse(ProductMax, out max))
                    {
                        break;
                    }
                    else
                    {
                        return "Must be a number";
                    }
                default:
                    return string.Empty;
            }
            return null;
        }
    }
}
