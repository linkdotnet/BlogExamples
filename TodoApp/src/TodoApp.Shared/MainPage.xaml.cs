using Newtonsoft.Json;
using System;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
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
            Loaded += RecoverState;
            LostFocus += (s, e) => SaveState();
        }

        private async void SaveState()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("todoapp.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(DataContext));
        }

        private async void RecoverState(object sender, RoutedEventArgs e)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.TryGetItemAsync("todoapp.json");
            if (file != null)
            {
                var text = await FileIO.ReadTextAsync(file as IStorageFile);
                var viewmodel = JsonConvert.DeserializeObject<MainPageViewModel>(text);
                if (viewmodel?.TodoItems.Any() == true)
                {
                    DataContext = viewmodel;
                }
            }            
        }
    }
}
