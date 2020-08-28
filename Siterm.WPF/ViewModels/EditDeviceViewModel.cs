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
        private readonly UserDataService _userDataService;
        private UserDraft _chiefUserDraft;
        private Device _device;

        private IEnumerable<string> _userEmailList;

        public EditDeviceViewModel(DeviceDataService deviceDataService, UserDataService userDataService)
        {
            _deviceDataService = deviceDataService;
            _userDataService = userDataService;
            ApplyChangesCommand = new RelayCommand(ApplyChanges);
        }

        public RelayCommand ApplyChangesCommand { get; }

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

        private async void ApplyChanges(object obj)
        {
            if (!ChiefUserDraft.IsValid) return;
            var user = await _userDataService.FirstOrDefault(u => u.Email == ChiefUserDraft.Email) ??
                       await _userDataService.Create(new User
                       {
                           FirstName = ChiefUserDraft.FirstName,
                           LastName = ChiefUserDraft.LastName,
                           Email = ChiefUserDraft.Email
                       });

            Device.Chief = user;
            await _deviceDataService.Update(Device.Id, Device);

            SentClosingRequest();
        }

        public async Task FetchDevice(int requestedDeviceId)
        {
            Device = await _deviceDataService.FirstFullOrDefault(d => d.Id == requestedDeviceId) ?? new Device();
            ChiefUserDraft = Device.Chief is null ? new UserDraft() : new UserDraft(Device.Chief);
        }

        public async Task FetchUsers()
        {
            UserEmailList = await _userDataService.GetAllEmails();
        }
    }
}