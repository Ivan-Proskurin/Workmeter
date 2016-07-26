using System;
using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Workmeter.ViewModels;

namespace Workmeter.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ITrayIcon
    {
        public MainView()
        {
            InitializeComponent();
            if (ViewModel == null) return;
            ViewModel.TrayIcon = this;
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
            //if (WindowState == WindowState.Minimized)
            //{
            //    Hide();
            //}
        }

        public void TaskbarIcon_OnPreviewTrayPopupOpen(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        public void Start_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.Start();
        }

        private void MainView_OnInitialized(object sender, EventArgs e)
        {
            Visibility = ViewModel?.StartHidden ?? true ? Visibility.Hidden : Visibility.Visible;
        }

        public void ShowInfoBallonTip(string title, string text)
        {
            trayIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
        }
    }
}
