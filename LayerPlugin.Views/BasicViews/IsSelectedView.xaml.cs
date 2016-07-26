using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerPlugin.Views.BasicViews
{
    public partial class IsSelectedView : UserControl
    {
        public Boolean IsSelected
        {
            get { return (Boolean)this.GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(Boolean), typeof(BasicViews.IsSelectedView), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IsSelectedView()
        {
            InitializeComponent();
        }
    }
}
