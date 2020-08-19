using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Siterm.Support.Misc;

namespace Siterm.Instructions.Models
{
    public class UserDraft : CanNotifyPropertyChanged
    {
        private string _email;

        private string _firstName;
        private string _input;

        private string _lastName;

        private Bitmap _signature;

        public string Input
        {
            get => _input;
            set => SetInput(value);
        }

        public Bitmap Signature
        {
            get => _signature;
            set => SetField(ref _signature, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetField(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetField(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public bool IsValid => Helper.IsValidEmail(Email);

        public string FileName =>
            $"{Email.ToLower(CultureInfo.CurrentCulture)}_{DateTime.Today.ToString("d", CultureInfo.CurrentCulture)}.pdf";

        public string FullName => $"{FirstName} {LastName}";

        private void SetInput(string value)
        {
            SetField(ref _input, value);
            

            if (!Helper.IsValidEmail(value)) return;
            Email = value;

            var mailPrefix = value.Split('@')[0];
            var nameSplit = mailPrefix.Split('.');

            if (nameSplit.Length < 2)
            {
                LastName = nameSplit[0];
            }
            else
            {
                FirstName = nameSplit[0];
                LastName = nameSplit.Last();
            }
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(FullName));
        }
    }
}