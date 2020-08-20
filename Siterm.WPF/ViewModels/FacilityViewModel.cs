using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Facility.Models;
using Siterm.Support.ControlModels;
using Siterm.Support.Misc;
using Siterm.WPF.State.Navigators;
using Siterm.WPF.Views;

namespace Siterm.WPF.ViewModels
{
    public class FacilityViewModel : BaseViewModel, ITabItemViewModel
    {
        private readonly DeviceDataService _deviceDataService;
        private readonly FacilityDataService _facilityDataService;
        private readonly SimpleNavigationService _navigationService;
        private string _deviceSearchTerm;
        private List<FacilityTreeViewItem> _facilities;
        private ICollectionView _facilityCollectionView;
        private bool _isLoadingFacilities;
        private Device _selectedDevice;

        public FacilityViewModel(FacilityDataService facilityDataService, DeviceDataService deviceDataService,
            SimpleNavigationService navigationService)
        {
            _facilityDataService = facilityDataService;
            _deviceDataService = deviceDataService;
            _navigationService = navigationService;
            RefreshFacilitiesCommand = new RelayCommand(RefetchFacilities);
            OnItemMouseDoubleClickCommand = new RelayCommand(ItemWasDoubleClicked);
            NewInstructionCommand = new RelayCommand(CreateNewInstruction);
            NewServiceReportCommand = new RelayCommand(CreateNewServiceReport);
        }

        public RelayCommand RefreshFacilitiesCommand { get; set; }
        public RelayCommand OnItemMouseDoubleClickCommand { get; }
        public RelayCommand NewInstructionCommand { get; }
        public RelayCommand NewServiceReportCommand { get; }

        public bool IsLoadingFacilities
        {
            get => _isLoadingFacilities;
            private set => SetField(ref _isLoadingFacilities, value);
        }

        public ICollectionView FacilityCollectionView
        {
            get
            {
                if (_facilityCollectionView is null) FetchFacilities();

                return _facilityCollectionView;
            }
            private set
            {
                SetField(ref _facilityCollectionView, value);
            }
        }

        public string DeviceSearchTerm
        {
            get => _deviceSearchTerm;
            set
            {
                SetField(ref _deviceSearchTerm, value);
                _facilityCollectionView?.Refresh();
            }
        }

        public Device SelectedDevice
        {
            get => _selectedDevice;
            set => SetField(ref _selectedDevice, value);
        }

        public string Header => UiStrings.FacilityTabHeader;
        public int Position => 3;

        public async void SelectedDeviceChanged(Device selectedDevice)
        {
            var loadedDevice = await _deviceDataService.GetFull(selectedDevice.Id);
            SelectedDevice = loadedDevice;
        }

        private async void CreateNewInstruction(object o)
        {
            await _navigationService.ShowDialogAsync<CreateInstructionView>(SelectedDevice.Id);
            SelectedDeviceChanged(SelectedDevice);
        }

        private async void CreateNewServiceReport(object o)
        {
            await _navigationService.ShowDialogAsync<CreateServiceReportView>(SelectedDevice.Id);
            SelectedDeviceChanged(SelectedDevice);
        }

        private async Task FetchFacilities()
        {
            IsLoadingFacilities = true;

            _facilities = (await _facilityDataService.GetAllWithDevices())
                .OrderBy(s => s.OrderNr)
                .Select(s => new FacilityTreeViewItem(s)).ToList();

            FacilityCollectionView = CollectionViewSource.GetDefaultView(_facilities);
            FacilityCollectionView.Filter = o =>
                string.IsNullOrEmpty(DeviceSearchTerm) ||
                ((FacilityTreeViewItem) o).Model.Name.Contains(DeviceSearchTerm,
                    StringComparison.CurrentCultureIgnoreCase);

            IsLoadingFacilities = false;
        }

        private void ItemWasDoubleClicked(object obj)
        {
            if (!(obj is TreeView facilityTreeView)) return;
            if (!(facilityTreeView.SelectedItem is File file)) return;

            file.Open();
        }

        private async void RefetchFacilities(object o)
        {
            await FetchFacilities();
        }
    }
}