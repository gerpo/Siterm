using System.Collections.Generic;
using Siterm.Domain.Models;
using Siterm.Instructions.Models;
using Siterm.Support.Misc;

namespace Siterm.WPF.ViewModels
{
    public class EditDeviceViewModel : BaseViewModel
    {
        private UserDraft _chiefUserDraft;
        private Device _device;

        private IEnumerable<string> _userEmailList;

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
    }
}