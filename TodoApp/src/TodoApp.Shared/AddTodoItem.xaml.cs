using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class AddTodoItem : UserControl
    {
        public AddTodoItem()
        {
            this.InitializeComponent();
        }

        private void BtnClick(object sender, RoutedEventArgs args)
        {
            var dialog = new AddTodoItemDialog();
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            dialog.ShowAsync();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = sender.DataContext as object;
        }
    }
}
