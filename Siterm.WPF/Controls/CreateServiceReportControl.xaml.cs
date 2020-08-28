using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Siterm.Domain.Models;
using Siterm.ServiceReports.Models;

namespace Siterm.WPF.Controls
{
    public partial class CreateServiceReportControl
    {
        public static readonly DependencyProperty DeviceListProperty = DependencyProperty.Register(
            nameof(DeviceList),
            typeof(IEnumerable<Device>),
            typeof(CreateServiceReportControl)
        );

        public static readonly DependencyProperty UserEmailListProperty = DependencyProperty.Register(
            nameof(UserEmailList),
            typeof(IEnumerable<string>),
            typeof(CreateServiceReportControl)
        );

        public static readonly DependencyProperty ServiceReportDraftProperty = DependencyProperty.Register(
            nameof(ServiceReportDraft),
            typeof(ServiceReportDraft),
            typeof(CreateServiceReportControl)
        );

        public CreateServiceReportControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Device> DeviceList
        {
            get => (IEnumerable<Device>) GetValue(DeviceListProperty);
            set => SetValue(DeviceListProperty, value);
        }
        public IEnumerable<string> UserEmailList
        {
            get => (IEnumerable<string>)GetValue(UserEmailListProperty);
            set => SetValue(UserEmailListProperty, value);
        }
        public ServiceReportDraft ServiceReportDraft
        {
            get => (ServiceReportDraft) GetValue(ServiceReportDraftProperty);
            set => SetValue(ServiceReportDraftProperty, value);
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid?.SelectedItem is ServiceTask item && e.Key == Key.Return)
            {
                item.IsDone = true;
            }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (!(dataGrid?.SelectedItem is ServiceTask item)) return;

            if (!(dataGrid.Columns[2].GetCellContent(item)?.Parent is DataGridCell cell)) return;
            cell.Focus();

        }

    }
}