using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using Siterm.Domain.Models;

namespace Siterm.WPF.Views
{
    public partial class FirstAidView : UserControl
    {
        public FirstAidView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                ((DataGridTextColumn) e.Column).Binding.StringFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            switch (e.PropertyName)
            {
                case nameof(FirstResponder.Name):
                    e.Column.Header = UiStrings.Name;
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    break;

                case nameof(FirstResponder.Facility):
                    e.Column.Header = UiStrings.Facility;
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    e.Column.SortDirection = ListSortDirection.Ascending;
                    break;

                case nameof(FirstResponder.Phone):
                    e.Column.Header = UiStrings.Phone;
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    break;

                case nameof(FirstResponder.LastTraining):
                    e.Column.Header = UiStrings.LastTraining;
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    break;

                case nameof(FirstResponder.NextTraining):
                    e.Column.Header = UiStrings.NextTraining;
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                    break;

                default:
                    e.Cancel = true;
                    break;
            }
        }
    }
}