using Microsoft.Toolkit.Uwp.UI;
using System;
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
            DataContextChanged += SetFilter;
        }

        private void SetFilter(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var view = new AdvancedCollectionView(((MainPageViewModel)this.DataContext).TodoItems, true);
            view.Filter = item => ((Todo)item).KanbanState == State;
            itemListView.ItemsSource = view;
        }

        public static readonly DependencyProperty KanbanStateProperty = DependencyProperty.Register(nameof(State), typeof(KanbanState), typeof(Swimlane), new PropertyMetadata(KanbanState.New));

        public KanbanState State
        {
            get { return (KanbanState)GetValue(KanbanStateProperty); }
            set { SetValue(KanbanStateProperty, value); }
        }
    }
}
