using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class AddTodoItemButton : UserControl
    {
        public AddTodoItemButton()
        {
            this.InitializeComponent();
        }

        private void BtnClick(object sender, RoutedEventArgs args)
        {
            var dialog = new AddTodoItemDialog();
            dialog.ShowAsync();
        }
    }
}
