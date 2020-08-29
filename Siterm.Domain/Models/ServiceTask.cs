#nullable enable
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Siterm.Domain.Models
{
    public class ServiceTask : DomainObject, INotifyPropertyChanged
    {
        private string _comment;
        private bool _isDone;
        public string Area { get; set; }
        public string Description { get; set; }

        public bool IsDone
        {
            get => _isDone;
            set => SetField(ref _isDone, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetField(ref _comment, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}