using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WGU.C968.InventoryManagement.Domain;

namespace WGU.C968.InventoryManagement.Views
{
    /// <summary>
    /// Interaction logic for AddEditPart.xaml
    /// </summary>
    public partial class AddEditPart : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        public Part Part { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string PartCost
        {
            get { return _partCost; }
            set { _partCost = value; OnPropertyChanged(nameof(PartCost)); }
        }
        public string PartCount
        {
            get { return _partCount; }
            set { _partCount = value; OnPropertyChanged(nameof(PartCount)); }
        }

        public string PartMin
        {
            get { return _partMin; }
            set { _partMin = value; OnPropertyChanged(nameof(PartMin)); OnPropertyChanged(nameof(PartCount)); }
        }

        public string PartMax
        {
            get { return _partMax; }
            set { _partMax = value; OnPropertyChanged(nameof(PartMax)); OnPropertyChanged(nameof(PartCount)); }
        }

        public string PartVariableValue
        {
            get { return _partVariableValue; }
            set { _partVariableValue = value; OnPropertyChanged(nameof(PartVariableValue)); }
        }

        public bool IsOutsourced 
        {
            get { return _isOutsourced; }
            set { _isOutsourced = value; OnPropertyChanged(nameof(IsOutsourced)); OnPropertyChanged(nameof(PartVariableValue)); } 
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

        private readonly MainWindow mainWindow;
        private readonly bool isNewPart;
        private bool _isOutsourced;
        private string _partMin;
        private string _partCount;
        private string _partMax;
        private string _partVariableValue;
        private string _partCost;
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Error => null;
        public string this[string propertyName] => GetPropertyError(propertyName);

        private static readonly string[] ValidatedProperties = { "PartName", "PartCost", "PartCount", "PartMax", "PartMin", "PartVariableValue" };

        public AddEditPart(int partId)
        {
            InitializeComponent();

            mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            Part = mainWindow.Model.LookupPart(partId);

            isNewPart = Part == null ? true : false;
            PartId = Part != null ? Part.PartId : partId;
            PartName = Part != null ? Part.Name : "";
            PartCost = Part != null ? Part.Price.ToString() : "";
            PartCount = Part != null ? Part.InStock.ToString() : "";
            PartMin = Part != null ? Part.Min.ToString() : "";
            PartMax = Part != null ? Part.Max.ToString() : "";
            PartVariableValue = Part == null ? "" : Part is OutsourcedPart 
                ? (Part as OutsourcedPart).CompanyName
                : (Part as InhousePart).MachineId.ToString();
            IsOutsourced = Part != null && Part.GetType() == typeof(OutsourcedPart) ? true : false;

            this.DataContext = this;    
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid)
            {
                if (IsOutsourced)
                {
                    Part = new OutsourcedPart
                    {
                        PartId = PartId,
                        Name = PartName,
                        Price = decimal.Parse(PartCost),
                        InStock = int.Parse(PartCount),
                        Max = int.Parse(PartMax),
                        Min = int.Parse(PartMin),
                        CompanyName = PartVariableValue
                    };
                }
                else
                {
                    Part = new InhousePart
                    {
                        PartId = PartId,
                        Name = PartName,
                        Price = decimal.Parse(PartCost),
                        InStock = int.Parse(PartCount),
                        Max = int.Parse(PartMax),
                        Min = int.Parse(PartMin),
                        MachineId = int.Parse(PartVariableValue)
                    };
                }

                if (isNewPart)
                {
                    mainWindow.Model.AddPart(Part);
                }
                else
                {
                    mainWindow.Model.UpdatePart(PartId, Part);
                }

                this.Close();
                mainWindow.Parts = mainWindow.GetUpdatedParts();
                mainWindow.Opacity = 1;
            }
            else
            {
                MessageBox.Show(
                    "You must correct the part errors before saving.",
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
                case "PartName":
                    if (string.IsNullOrEmpty(PartName))
                        return "Required";
                    break;
                case "PartCount":
                    int.TryParse(PartMin, out min);
                    int.TryParse(PartMax, out max);

                    if (string.IsNullOrWhiteSpace(PartCount))
                        return "Required";
                    else if (int.TryParse(PartCount, out count))
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
                case "PartCost":
                    if (string.IsNullOrWhiteSpace(PartCost))
                    {
                        return "Required";
                    }
                    else if (decimal.TryParse(PartCost, out decimal cost)
                        && cost >= 0
                        && cost * 100 == Math.Floor(cost * 100))
                    {
                        return null;
                    }
                    else
                    {
                        return "Must be a price";
                    }
                case "PartMin":
                    int.TryParse(PartMin, out min);
                    int.TryParse(PartMax, out max);

                    if (string.IsNullOrWhiteSpace(PartMin))
                        return "Required";
                    else if (int.TryParse(PartMin, out count))
                    {
                        if (count < min || count > max)
                        {
                            return $"Must be less than {PartMax}";
                        }
                    }
                    else
                    {
                        return "Must be a number";
                    }
                    break;
                case "PartMax":
                    if (string.IsNullOrWhiteSpace(PartMax))
                    {
                        return "Required";
                    }
                    else if (int.TryParse(PartMax, out max))
                    {
                        return null;
                    }
                    else
                    {
                        return "Must be a number";
                    }
                case "PartVariableValue":
                    if (string.IsNullOrWhiteSpace(PartVariableValue))
                    {
                        return "Required";
                    }
                    else if (!IsOutsourced)
                    {
                        if (int.TryParse(PartVariableValue, out int machineId))
                        {
                            return null;
                        }
                        else
                        {
                            return "Must be a number";
                        }
                    }
                    break;
                default:
                    return string.Empty;
            }
            return null;
        }
    }
}
