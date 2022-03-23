using System;
using TodoApp.Domain;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class AddTodoItemButton : UserControl
    {
        public AddTodoItemButton()
        {
            InitializeComponent();
        }

        public event EventHandler<Todo> TodoItemCreated;

        private void OpenDialog(object sender, RoutedEventArgs args)
        {
            var dialog = new AddTodoItemDialog();
            dialog.PrimaryButtonClick += (s, a) => NewTodoItemCreated((NewTodoItemViewModel)dialog.DataContext);
            dialog.ShowAsync();
        }

        private void NewTodoItemCreated(NewTodoItemViewModel viewModel)
        {
            var todo = new Todo
            {
                Description = viewModel.Description,
                Title = viewModel.Title,
                DueDate = viewModel.DueDate.DateTime,
                KanbanState = KanbanState.New,
            };

            TodoItemCreated?.Invoke(this, todo);
        }
    }
}
