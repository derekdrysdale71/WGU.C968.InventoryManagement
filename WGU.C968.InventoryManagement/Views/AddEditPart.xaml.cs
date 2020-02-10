using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddEditPart.xaml
    /// </summary>
    public partial class AddEditPart : Window
    {
        public AddEditPart(int? partId = null)
        {
            InitializeComponent();

            if (partId == null)
            {
                //var newPart = new Part();
            }
        }

        // Window Actions
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
