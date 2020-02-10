using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WGU.C968.InventoryManagement.Domain;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var model = new Inventory();

            this.DataContext = model;

            PartsDataGrid.ItemsSource = model.Parts;
            ProductsDataGrid.ItemsSource = model.Products;
        }

        // Part Actions
        private void PartSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (PartSearchTextBox.Text != "")
            {
                try
                {
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("No part matches that search value.", "Result Error", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Please enter a value to search for.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditPart addPartWindow = new AddEditPart();
            addPartWindow.Owner = Window.GetWindow(this);
            addPartWindow.Show();
        }

        private void ModifyPartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Product Actions
        private void ProductSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductSearchTextBox.Text != "")
            {
                try
                {

                }
                catch (Exception)
                {
                    MessageBox.Show("No product matches that search value.", "Result Error", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Please enter a value to search for.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditProduct addProductWindow = new AddEditProduct();
            addProductWindow.Owner = Window.GetWindow(this);
            addProductWindow.Show();
        }

        // MainWindow Actions
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
