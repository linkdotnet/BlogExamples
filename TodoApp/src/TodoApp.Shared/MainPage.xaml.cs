using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();
            addItemButton.TodoItemCreated += (o, item) => ((MainPageViewModel)DataContext).TodoItems.Add(item);
        }
    }
}
