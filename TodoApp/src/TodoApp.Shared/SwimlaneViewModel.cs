
using MvvmHelpers;
using TodoApp.Domain;

namespace TodoApp
{
    public class SwimlaneViewModel : ObservableObject
    {
        private ObservableRangeCollection<Todo> todoItems;

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
