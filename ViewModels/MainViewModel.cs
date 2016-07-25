using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Workmeter.Annotations;
using Workmeter.Models;
using Workmeter.Views;

namespace Workmeter.ViewModels
{
    [SuppressMessage("ReSharper", "InvertIf")]
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WorkmeterDb _db;

        public bool StartHidden =>
            Environment.GetCommandLineArgs().Any(arg => arg == "/hidden");

        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                _items = new ObservableCollection<TaskItemViewModel>()
                {
                    new TaskItemViewModel(new WorkmeterTask()
                    {
                        State = TaskState.Paused,
                        Title = "Задача №1",
                        Duration = new TimeSpan(2, 30, 45)
                    }),
                    new TaskItemViewModel(new WorkmeterTask()
                    {
                        State = TaskState.Stopped,
                        Title = "Задача №2",
                        Duration = new TimeSpan(1, 23, 22)
                    }),
                    new TaskItemViewModel(null)
                };
            }
            else
            {
                _db = WorkmeterDb.Instance;
                _items = new ObservableCollection<TaskItemViewModel>(
                    _db.Tasks.Where(t => t.State != TaskState.Hidden).Select(ItemViewModel));
                _items.Add(new TaskItemViewModel(null));
            }

            _items.CollectionChanged += ItemsOnCollectionChanged;
        }

        private TaskItemViewModel ItemViewModel(WorkmeterTask task)
        {
            var viewModel = new TaskItemViewModel(task);
            viewModel.PropertyChanged += ItemOnPropertyChanged;
            return viewModel;
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TaskItemViewModel item;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    item = e.NewItems[0] as TaskItemViewModel;
                    if (item != null)
                        item.PropertyChanged += ItemOnPropertyChanged;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    item = e.OldItems[0] as TaskItemViewModel;
                    if (item != null)
                        item.PropertyChanged -= ItemOnPropertyChanged;
                    break;
            }
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TaskItemViewModel item;
            if (e.PropertyName == nameof(item.Duration))
            {
                OnPropertyChanged(nameof(DurationTotal));
            }
        }

        private ObservableCollection<TaskItemViewModel> _items;
        public ObservableCollection<TaskItemViewModel> Items => _items;

        private TaskItemViewModel _selectedItem;
        public TaskItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public void SwitchItem()
        {
            if (_selectedItem.IsNew)
            {
                NewTask();
                return;
            }
            var active = _items.SingleOrDefault(x => !x.IsNew && (x.Model.State == TaskState.Active || x.Model.State == TaskState.Paused));
            if (active != null && active == _selectedItem)
            {
                if (active.Model.State == TaskState.Active)
                {
                    active.Pause();
                }
                else
                {
                    active.Start();
                }
            }
            else
            {
                active?.Stop();
                _selectedItem?.Start();
            }
        }

        public void NewTask()
        {
            var newTask = new NewTaskView() { Owner = Application.Current.MainWindow };
            if (newTask.ShowDialog() != true) return;
            var newTaskVm = newTask.DataContext as NewTaskViewModel;
            if (newTaskVm == null) return;
            var task = newTaskVm.GetTask();
            _db.Tasks.Add(task);
            var taskVm = new TaskItemViewModel(task);
            _items.Insert(_items.Count - 1, taskVm);
            if (!newTaskVm.SwitchToIt) return;
            SelectedItem = taskVm;
            SwitchItem();
        }

        public void CloseDb()
        {
            var active = _items.SingleOrDefault(x => !x.IsNew && x.Model.State == TaskState.Active);
            active?.Pause();
            _db?.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Hide()
        {
            if (_selectedItem == null || _selectedItem.IsNew) return;
            if (_selectedItem.Model.State == TaskState.Active)
            {
                _selectedItem.Stop();
            }
            _selectedItem.Model.State = TaskState.Hidden;
            _items.Remove(_selectedItem);
        }

        public void ResetAll()
        {
            if (MessageBox.Show(Application.Current.MainWindow, "Сбросить все задачи?",
                "Workmeter", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) return;
            foreach (var item in _items.Where(item => !item.IsNew))
            {
                item.Reset();
            }
        }

        public void ReturnTask()
        {
            var view = new ReturnTaskView() { Owner = Application.Current.MainWindow };
            if (view.ShowDialog() != true) return;
            var viewModel = view.DataContext as ReturnTaskViewModel;
            var task = viewModel?.SelectedItem;
            if (task == null) return;
            task.State = TaskState.Stopped;
            var item = new TaskItemViewModel(task);
            var insertIndex = _db.Tasks.Where(t => t.State != TaskState.Hidden).ToList().IndexOf(task);
            _items.Insert(insertIndex, item);
            if (viewModel.ResetIt)
            {
                item.Reset();
            }
            if (!viewModel.SwitchToIt) return;
            SelectedItem = item;
            SwitchItem();
        }

        public void Start()
        {
            if (MessageBox.Show(Application.Current.MainWindow, "Сбросить все задачи и начать отсчет?",
                "Workmeter", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) return;
            var active = _items.SingleOrDefault(item => !item.IsNew && (item.Model.State == TaskState.Active || item.Model.State == TaskState.Paused));
            active?.Stop();
            foreach (var item in _items.Where(item => !item.IsNew))
            {
                item.Reset();
            }
            _selectedItem?.Start();
        }

        public TimeSpan DurationTotal => 
            new TimeSpan(_items.Where(item => !item.IsNew).Sum(item => item.Duration?.Ticks ?? 0));

    }
}