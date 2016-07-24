using System.Windows;
using System.Windows.Input;
using Workmeter.ViewModels;

namespace Workmeter.Views
{
    /// <summary>
    /// Interaction logic for ReturnTaskView.xaml
    /// </summary>
    public partial class ReturnTaskView
    {
        public ReturnTaskView()
        {
            InitializeComponent();
        }

        public void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            Commit();
        }

        private void Commit()
        {
            if ((DataContext as ReturnTaskViewModel)?.Validate() != true) return;
            DialogResult = true;
            Close();
        }

        public void Item_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            Commit();
        }
    }
}
