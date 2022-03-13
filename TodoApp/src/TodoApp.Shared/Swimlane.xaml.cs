using TodoApp.Domain;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class Swimlane : UserControl
    {
        public Swimlane()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty KanbanStateProperty = DependencyProperty.Register(nameof(State), typeof(KanbanState), typeof(Swimlane), new PropertyMetadata(KanbanState.New));

        public KanbanState State
        {
            get { return (KanbanState)GetValue(KanbanStateProperty); }
            set { SetValue(KanbanStateProperty, value); }
        }
    }
}
