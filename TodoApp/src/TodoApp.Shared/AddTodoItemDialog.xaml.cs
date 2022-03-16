using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class AddTodoItemDialog : ContentDialog
	{
		private readonly NewTodoItemViewModel viewModel = new NewTodoItemViewModel();

		public AddTodoItemDialog()
		{
			InitializeComponent();
			DataContext = viewModel;
			viewModel.PropertyChanged += (s, a) => IsPrimaryButtonEnabled = viewModel.IsViewModelValid;
		}
	}
}
