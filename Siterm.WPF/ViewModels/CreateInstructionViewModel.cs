using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Accessibility;
using MahApps.Metro.Controls.Dialogs;
using Org.BouncyCastle.Cms;
using Siterm.Domain.Models;
using Siterm.EntityFramework.Services;
using Siterm.Instructions.Models;
using Siterm.Instructions.Services;
using Siterm.Signature.Exceptions;
using Siterm.Signature.Models;
using Siterm.Signature.Resources;
using Siterm.Signature.Services;
using Siterm.Signature.Views;
using Siterm.Support.Misc;
using Siterm.WPF.State.Navigators;

namespace Siterm.WPF.ViewModels
{
    public class CreateInstructionViewModel : BaseViewModel
    {
        private readonly DeviceDataService _deviceDataService;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly SimpleNavigationService _navigationService;
        private readonly SignatureService _signatureService;
        private readonly InstructionService _instructionService;
        private readonly UserDataService _userDataService;
        private ObservableCollection<Device> _deviceList;


        private InstructionDraft _instructionDraft;
        private Device _selectedDevice;

        private IEnumerable<string> _userEmailList;

        public CreateInstructionViewModel(DeviceDataService deviceDataService, UserDataService userDataService,
            SignatureService signatureService, InstructionService instructionService,
            IDialogCoordinator dialogCoordinator, SimpleNavigationService navigationService)
        {
            _deviceDataService = deviceDataService;
            _userDataService = userDataService;
            _signatureService = signatureService;
            _instructionService = instructionService;
            _dialogCoordinator = dialogCoordinator;
            _navigationService = navigationService;


            CreateInstructionCommand = new RelayCommand(CreateInstruction);
        }

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

        public Device SelectedDevice
        {
            get => _selectedDevice;
            set => SetField(ref _selectedDevice, value);
        }

        public InstructionDraft InstructionDraft
        {
            get => _instructionDraft ??= new InstructionDraft();
            set => SetField(ref _instructionDraft, value);
        }

        public RelayCommand CreateInstructionCommand { get; set; }

        public async void FetchDevices(int requestedDeviceId = -1)
        {
            DeviceList = new ObservableCollection<Device>(await _deviceDataService.GetAll());

            if (requestedDeviceId < 1)
                return;

            InstructionDraft.Device = DeviceList.FirstOrDefault(d => d.Id == requestedDeviceId);
        }

        public async void FetchUsers()
        {
            UserEmailList = await _userDataService.GetAllEmails();
        }

        private async void CreateInstruction(object obj)
        {
            var signaturesCollected = await GetSignatures();

            if (!signaturesCollected) return;

            _instructionService.CreateInstruction(InstructionDraft);
        }

        private async Task<bool> GetSignatures()
        {
            var signatureBundles =
                InstructionDraft.Instructed.Select(i => new SignatureBundle(i, SignatureCause.Instruction)).ToList();
            signatureBundles.Add(new SignatureBundle(InstructionDraft.Instructor, SignatureCause.Instructor));

            try
            {
                for (var index = 0; index < signatureBundles.Count; index++)
                {
                    var signatureBundle = signatureBundles[index];
                    var signatureDialogResult =
                        await _navigationService.ShowDialogAsync<SignatureView>(signatureBundle) ?? false;

                    if (signatureDialogResult) continue;

                    var cancelConfirmationResult = await _dialogCoordinator.ShowMessageAsync(this,
                        UiStrings.RetrySignatureTitle,
                        UiStrings.RetrySignatureMessage,
                        MessageDialogStyle.AffirmativeAndNegative);

                    switch (cancelConfirmationResult)
                    {
                        case MessageDialogResult.Affirmative:
                            index--;
                            break;
                        case MessageDialogResult.Canceled when signatureBundle.Cause == SignatureCause.Instructor:
                            return false;
                    }
                }
            }
            catch (NoSignaturePadException)
            {
                await _dialogCoordinator.ShowMessageAsync(this, UiStrings.NoSignaturePadExceptionTitle,
                    UiStrings.NoSignaturePadExceptionMessage);
            }

            return true;
        }
    }
}