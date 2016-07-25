using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LayerPlugin.Views.BasicViews
{
    public partial class HeightView : UserControl
    {
        public Double Height
        {
            get { return (Double)this.GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(Double), typeof(BasicViews.HeightView), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public HeightView()
        {
            InitializeComponent();
        }
    }
}
