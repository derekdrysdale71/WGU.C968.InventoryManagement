using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        private readonly MainWindow mainWindow;

        public AddEditProduct()
        {
            InitializeComponent();

            mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }


        private void PartSearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            //GetUpdatedParts();
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Opacity = 1;
        }
    }
}
