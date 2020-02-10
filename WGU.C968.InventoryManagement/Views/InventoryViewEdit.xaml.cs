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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for InventoryViewEdit.xaml
    /// </summary>
    public partial class InventoryViewEdit : UserControl
    {
        public InventoryViewEdit()
        {
            InitializeComponent();

            //this.DataContext = this;
            LayoutRoot.DataContext = this;
        }

        public string LabelText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(InventoryViewEdit), new PropertyMetadata(null));
    }
}
