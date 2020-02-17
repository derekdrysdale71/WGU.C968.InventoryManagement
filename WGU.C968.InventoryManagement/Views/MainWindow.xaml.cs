using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WGU.C968.InventoryManagement.Domain;
using WGU.C968.InventoryManagement.Helpers;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Inventory Model { get; set; }
        public Part SelectedPart { get; set; }
        public Product SelectedProduct { get; set; }
        public ObservableCollection<Part> Parts
        {
            get { return _parts; }
            set { _parts = value; OnPropertyChanged(nameof(Parts)); }
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(nameof(Products)); }
        }

        private ObservableCollection<Part> _parts;
        private ObservableCollection<Product> _products;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            Model = new Inventory();
            Parts = GetUpdatedParts();
            Products = GetUpdatedProducts();
        }

        // Part Actions
        private void PartSearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            Parts = GetUpdatedParts();
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            var partId = GetPartIds(Model.Parts).GetNextId();
            AddEditPart addPartWindow = new AddEditPart(partId);
            addPartWindow.Owner = Window.GetWindow(this);
            Opacity = 0.75;
            addPartWindow.ShowDialog();
        }

        private void ModifyPartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPart != null)
            {
                AddEditPart modifyPartWindow = new AddEditPart(SelectedPart.PartId);
                modifyPartWindow.Owner = Window.GetWindow(this);
                Opacity = 0.75;
                modifyPartWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                    "You must select a part to modify.",
                    "No Part Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void DeletePartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPart != null)
            {
                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this part?  This can't be undone.", 
                    "Warning",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning
                );

                if (confirm == MessageBoxResult.OK)
                {
                    Model.DeletePart(SelectedPart);
                    Parts = GetUpdatedParts();
                }
                else return;
            }
            else
            {
                MessageBox.Show(
                    "You must select a part to delete.",
                    "No Part Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        // Product Actions
        private void ProductSearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            Products = GetUpdatedProducts();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var productId = GetProductIds(Model.Products).GetNextId();
            AddEditProduct addProductWindow = new AddEditProduct(productId, Model);
            addProductWindow.Owner = Window.GetWindow(this);
            Opacity = 0.75;
            addProductWindow.ShowDialog();
        }

        private void ModifyProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                AddEditProduct modifyProductWindow = new AddEditProduct(SelectedProduct.ProductId, Model);
                modifyProductWindow.Owner = Window.GetWindow(this);
                Opacity = 0.75;
                modifyProductWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show(
                    "You must select a product to modify.",
                    "No Product Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this product?  This can't be undone.",
                    "Warning",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning
                );

                if (confirm == MessageBoxResult.OK)
                {
                    try
                    {
                        Model.RemoveProduct(SelectedProduct.ProductId);
                        Products = GetUpdatedProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "You must select a product to delete.",
                    "No Product Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        // MainWindow Actions
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private List<int> GetPartIds(IEnumerable<Part> parts)
        {
            return parts.Select(p => p.PartId).ToList();
        }

        private List<int> GetProductIds(IEnumerable<Product> products)
        {
            return products.Select(p => p.ProductId).ToList();
        }

        public ObservableCollection<Part> GetUpdatedParts()
        {
            return new ObservableCollection<Part>(Model.Parts);
        }

        public ObservableCollection<Product> GetUpdatedProducts()
        {
           return new ObservableCollection<Product>(Model.Products);
        }
    }
}
