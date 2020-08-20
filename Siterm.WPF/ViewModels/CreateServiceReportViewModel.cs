using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.ServiceReports.Models;
using Siterm.ServiceReports.Services;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class CreateServiceReportViewModel : BaseViewModel
    {
        private readonly DeviceDataService _deviceDataService;
        private readonly ServiceReportDraftFactory _serviceReportDraftFactory;

        private ObservableCollection<Device> _deviceList;
        
        private ServiceReportDraft _serviceReportDraft;

        public CreateServiceReportViewModel(DeviceDataService deviceDataService,
            ServiceReportDraftFactory serviceReportDraftFactory)
        {
            _deviceDataService = deviceDataService;
            _serviceReportDraftFactory = serviceReportDraftFactory;
        }

        public ObservableCollection<Device> DeviceList
        {
            get => _deviceList;
            set => SetField(ref _deviceList, value);
        }
        
        public ServiceReportDraft ServiceReportDraft
        {
            get => _serviceReportDraft ??= _serviceReportDraftFactory.CreateServiceReportDraft();
            set => SetField(ref _serviceReportDraft, value);
        }

        public async Task FetchDevices(int requestedDeviceId = -1)
        {
            DeviceList = new ObservableCollection<Device>(await _deviceDataService.GetAll());

            if (requestedDeviceId < 1)
                return;

            ServiceReportDraft.Device = DeviceList.FirstOrDefault(d => d.Id == requestedDeviceId);
        }
    }
}