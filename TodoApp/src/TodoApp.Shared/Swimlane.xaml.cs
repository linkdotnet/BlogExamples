using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Linq;
using TodoApp.Domain;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp
{
    public sealed partial class Swimlane : UserControl
    {
        private AdvancedCollectionView view;

        public Swimlane()
        {
            InitializeComponent();
            DataContextChanged += SetFilter;
        }

        public static readonly DependencyProperty KanbanStateProperty = DependencyProperty.Register(nameof(State), typeof(KanbanState), typeof(Swimlane), new PropertyMetadata(KanbanState.New));

        public KanbanState State
        {
            get { return (KanbanState)GetValue(KanbanStateProperty); }
            set { SetValue(KanbanStateProperty, value); }
        }

        private void SetFilter(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            view = new AdvancedCollectionView(((MainPageViewModel)this.DataContext).TodoItems, true);
            view.Filter = item => ((Todo)item).KanbanState == State;
            itemListView.ItemsSource = view;
        }

        private void SetDragItem(object sender, DragItemsStartingEventArgs e)
        {
            var draggedItem = e.Items.FirstOrDefault() as Todo;
            e.Data.SetText(draggedItem.Id.ToString());
        }

        private async void DropItem(object sender, DragEventArgs e)
        {
            var todoId = Guid.Parse(await e.DataView.GetTextAsync());
            var todo = ((MainPageViewModel)DataContext).TodoItems.SingleOrDefault(t => t.Id == todoId);
            todo.KanbanState = State;
            view.Refresh();
        }

        private void SetDragOverIcon(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private void UpdateList(object sender, DragItemsCompletedEventArgs e)
        {
            view.Refresh();
        }
    }
}
