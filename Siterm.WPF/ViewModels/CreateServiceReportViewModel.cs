using System;
using System.Collections.Generic;
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
        private readonly UserDataService _userDataService;
        private readonly ServiceReportService _serviceReportService;

        private ObservableCollection<Device> _deviceList;

        private ServiceReportDraft _serviceReportDraft;
        private IEnumerable<string> _userEmailList;

        public CreateServiceReportViewModel(DeviceDataService deviceDataService,
            ServiceReportDraftFactory serviceReportDraftFactory, UserDataService userDataService,ServiceReportService serviceReportService)
        {
            _deviceDataService = deviceDataService;
            _serviceReportDraftFactory = serviceReportDraftFactory;
            _userDataService = userDataService;
            _serviceReportService = serviceReportService;

            CreateServiceReportCommand = new RelayCommand(CreateServiceReport);
        }

        public RelayCommand CreateServiceReportCommand { get; set; }

        public ObservableCollection<Device> DeviceList
        {
            get => _deviceList;
            set => SetField(ref _deviceList, value);
        }

        public IEnumerable<string> UserEmailList
        {
            get => _userEmailList;
            set => SetField(ref _userEmailList, value);
        }

        public ServiceReportDraft ServiceReportDraft
        {
            get => _serviceReportDraft ??= _serviceReportDraftFactory.CreateServiceReportDraft();
            set => SetField(ref _serviceReportDraft, value);
        }

        private void CreateServiceReport(object obj)
        {
            _serviceReportService.CreateServiceReport(ServiceReportDraft);
        }

        public async Task FetchDevices(int requestedDeviceId = -1)
        {
            DeviceList = new ObservableCollection<Device>(await _deviceDataService.GetAll());

            if (requestedDeviceId < 1)
                return;

            ServiceReportDraft.Device = DeviceList.FirstOrDefault(d => d.Id == requestedDeviceId);
        }

        public async Task FetchUsers()
        {
            UserEmailList = await _userDataService.GetAllEmails();
        }
    }
}