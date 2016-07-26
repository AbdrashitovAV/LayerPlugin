using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerPlugin.Views.BasicViews
{
    public partial class HeightView : UserControl
    {
        public Double ModelHeight
        {
            get { return (Double)this.GetValue(ModelHeightProperty); }
            set { SetValue(ModelHeightProperty, value); }
        }
        public static readonly DependencyProperty ModelHeightProperty = DependencyProperty.Register("ModelHeight", typeof(Double), typeof(BasicViews.HeightView), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HeightView()
        {
            InitializeComponent();
        }
    }
}
