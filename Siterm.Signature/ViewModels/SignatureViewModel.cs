using System;
using System.Drawing;
using Siterm.Instructions.Models;
using Siterm.Signature.Resources;
using Siterm.Support.Misc;

namespace Siterm.Signature.ViewModels
{
    public class SignatureViewModel : BaseViewModel
    {
        private Bitmap _signature;

        private SignatureCause _signatureCause;
        private UserDraft _userDraft;

        public SignatureViewModel()
        {
            CancelSignatureCommand = new RelayCommand(CancelSignature);
        }

        public UserDraft UserDraft
        {
            get => _userDraft;
            set => SetField(ref _userDraft, value);
        }

        public SignatureCause SignatureCause
        {
            get => _signatureCause;
            set => SetField(ref _signatureCause, value);
        }

        public RelayCommand CancelSignatureCommand { get; set; }

        public Bitmap Signature
        {
            get => _signature;
            set
            {
                SetField(ref _signature, value);
                UserDraft.Signature = value;
                OnPropertyChanged(nameof(CanBeConfirmed));
            }
        }

        public bool CanBeConfirmed => UserDraft.Signature != null;

        private void CancelSignature(object obj)
        {
        }
    }
}