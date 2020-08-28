using System.Collections.Generic;
using System.Windows;
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

        public ServiceReportDraft ServiceReportDraft
        {
            get => (ServiceReportDraft) GetValue(ServiceReportDraftProperty);
            set => SetValue(ServiceReportDraftProperty, value);
        }
    }
}