using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Siterm.Domain.Models;
using Siterm.Support.Misc;

namespace Siterm.Instructions.Models
{
    public class InstructionDraft : CanNotifyPropertyChanged
    {
        private Device _device;
        private string _exceptFor;
        private ObservableCollection<UserDraft> _instructed;
        private ObservableCollection<Bitmap> _instructedSignature;
        private UserDraft _instructor;
        private Bitmap _instructorSignature;
        private string _onlyFor;

        public Device Device
        {
            get => _device;
            set
            {
                SetField(ref _device, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string OnlyFor
        {
            get => _onlyFor;
            set => SetField(ref _onlyFor, value);
        }

        public string ExceptFor
        {
            get => _exceptFor;
            set => SetField(ref _exceptFor, value);
        }

        public UserDraft Instructor
        {
            get => _instructor ??= new UserDraft();
            set
            {
                SetField(ref _instructor, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public ObservableCollection<UserDraft> Instructed
        {
            get => _instructed ??= new ObservableCollection<UserDraft>{new UserDraft()};
            set {
                SetField(ref _instructed, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public Bitmap InstructorSignature
        {
            get => _instructorSignature;
            set => SetField(ref _instructorSignature, value);
        }

        public ObservableCollection<Bitmap> InstructedSignature
        {
            get => _instructedSignature;
            set => SetField(ref _instructedSignature, value);
        }

        public bool IsValid => !(Device is null) && Instructor.IsValid && Instructed.All(i => i.IsValid);
    }
}