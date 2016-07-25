using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Threading;
using Workmeter.Annotations;
using Workmeter.Models;

namespace Workmeter.ViewModels
{
    public class TaskItemViewModel : INotifyPropertyChanged
    {
        public TaskItemViewModel(WorkmeterTask model)
        {
            Model = model;
            _timer.Tick += Timer_Tick;
        }

        public WorkmeterTask Model { get; set; }

        public string Text => IsNew ? "Новая задача" : Model.Title;

        public Brush Foreground
        {
            get
            {
                if (IsNew) return Brushes.White;
                switch (Model.State)
                {
                    case TaskState.Stopped:
                        return Brushes.Orange;
                    case TaskState.Active:
                        return Brushes.LightGreen;
                    case TaskState.Paused:
                        return Brushes.LightBlue;
                    case TaskState.Hidden:
                        return Brushes.White;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Model.State), Model.State, null);
                }
            }
        }

        public bool IsNew => Model == null;

        public TimeSpan? Duration
        {
            get
            {
                if (IsNew) return null;
                return Model.State == TaskState.Active
                    ? Model.Duration.Add(DateTime.Now.Subtract(_startTime ?? DateTime.Now))
                    : Model.Duration;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DateTime? _startTime;
        private readonly DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 1),
            IsEnabled = false
        };

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Duration));
        }

        private void NotifyItemChanged()
        {
            OnPropertyChanged(nameof(Model));
            OnPropertyChanged(nameof(Foreground));
            OnPropertyChanged(nameof(Duration));
        }

        public void Start()
        {
            if (IsNew) return;
            Model.State = TaskState.Active;
            _startTime = DateTime.Now;
            _timer.IsEnabled = true;
            NotifyItemChanged();
        }

        public void Stop()
        {
            if (IsNew) return;
            Model.Duration = Duration ?? TimeSpan.Zero;
            Model.State = TaskState.Stopped;
            _timer.IsEnabled = false;
            _startTime = null;
            NotifyItemChanged();
        }

        public void Pause()
        {
            if (IsNew) return;
            Model.Duration = Duration ?? TimeSpan.Zero;
            Model.State = TaskState.Paused;
            _timer.IsEnabled = false;
            _startTime = null;
            NotifyItemChanged();
        }

        public void Reset()
        {
            if (IsNew) return;
            if (Model.State == TaskState.Active)
            {
                Pause();
            }
            Model.Duration = TimeSpan.Zero;
            NotifyItemChanged();
        }
    }
}