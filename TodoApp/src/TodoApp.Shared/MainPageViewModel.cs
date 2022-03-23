using MvvmHelpers;
using TodoApp.Domain;

namespace TodoApp
{
    public class MainPageViewModel : ObservableObject
    {
        private ObservableRangeCollection<Todo> todoItems = new ObservableRangeCollection<Todo>();

        public ObservableRangeCollection<Todo> TodoItems
        {
            get => todoItems;
            set
            {
                todoItems = value;
                OnPropertyChanged();
            }
        }
    }
}
