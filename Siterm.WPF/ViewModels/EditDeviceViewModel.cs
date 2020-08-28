using System.Collections.Generic;
using System.Threading.Tasks;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Instructions.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class EditDeviceViewModel : BaseViewModel
    {
        private readonly DeviceDataService _deviceDataService;
        private UserDraft _chiefUserDraft;
        private Device _device;

        private IEnumerable<string> _userEmailList;

        public EditDeviceViewModel(DeviceDataService deviceDataService)
        {
            _deviceDataService = deviceDataService;
        }

        public Device Device
        {
            get => _device;
            set => SetField(ref _device, value);
        }

        public UserDraft ChiefUserDraft
        {
            get => _chiefUserDraft;
            set => SetField(ref _chiefUserDraft, value);
        }

        public IEnumerable<string> UserEmailList
        {
            get => _userEmailList;
            set => SetField(ref _userEmailList, value);
        }

        public async Task FetchDevice(int requestedDeviceId)
        {
            Device = await _deviceDataService.FirstOrDefault(d => d.Id == requestedDeviceId) ?? new Device();
        }
    }
}