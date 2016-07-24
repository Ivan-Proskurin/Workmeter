using System.Windows;
using Workmeter.ViewModels;

namespace Workmeter.Views
{
    /// <summary>
    /// Interaction logic for NewTaskView.xaml
    /// </summary>
    public partial class NewTaskView
    {
        public NewTaskView()
        {
            InitializeComponent();
            txtTitle.Focus();
            txtTitle.SelectAll();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewTaskViewModel;
            if (!viewModel?.Validate() ?? true) return;
            DialogResult = true;
            Close();
        }
    }
}
