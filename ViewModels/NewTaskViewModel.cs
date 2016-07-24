using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Workmeter.Annotations;
using Workmeter.Models;

namespace Workmeter.ViewModels
{
    public class NewTaskViewModel : INotifyPropertyChanged
    {
        private string _title = "Новая задача";
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private bool _switchToIt = true;
        public bool SwitchToIt
        {
            get { return _switchToIt; }
            set
            {
                if (_switchToIt == value) return;
                _switchToIt = value;
                OnPropertyChanged(nameof(SwitchToIt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WorkmeterTask GetTask()
        {
            return new WorkmeterTask()
            {
                State = TaskState.Stopped,
                Duration = TimeSpan.Zero,
                Title = Title
            };
        }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(_title);
        }
    }
}