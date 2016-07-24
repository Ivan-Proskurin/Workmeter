using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Workmeter.Annotations;
using Workmeter.Models;

namespace Workmeter.ViewModels
{
    public class ReturnTaskViewModel : INotifyPropertyChanged
    {
        public ReturnTaskViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Items = new ObservableCollection<WorkmeterTask>(
                    WorkmeterDb.Instance.Tasks.Where(t => t.State == TaskState.Hidden)
                    );
            }
        }

        public ObservableCollection<WorkmeterTask> Items { get; set; }

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

        private bool _resetIt = true;
        public bool ResetIt
        {
            get { return _resetIt; }
            set
            {
                if (_resetIt == value) return;
                _resetIt = value;
                OnPropertyChanged(nameof(ResetIt));
            }
        }

        private WorkmeterTask _selectedItem;
        public WorkmeterTask SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Validate()
        {
            return _selectedItem != null;
        }
    }
}