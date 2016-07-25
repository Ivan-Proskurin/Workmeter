using System;
using System.Windows;
using System.Windows.Input;
using Workmeter.ViewModels;

namespace Workmeter.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;

        public void Item_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            ViewModel?.SwitchItem();
        }

        public void MainView_OnClosed(object sender, EventArgs e)
        {
            ViewModel?.CloseDb();
        }

        public void Hide_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.Hide();
        }

        public void ResetAll_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.ResetAll();
        }

        public void ReturnTask_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.ReturnTask();
        }

        public void MainView_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        public void TaskbarIcon_OnPreviewTrayPopupOpen(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            BringIntoView();
        }

        public void Start_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.Start();
        }

        private void MainView_OnInitialized(object sender, EventArgs e)
        {
            Visibility = ViewModel?.StartHidden ?? true ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
