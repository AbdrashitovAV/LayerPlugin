using System;
using System.Windows;
using System.Windows.Controls;

namespace LayerPlugin.Views.BasicViews
{
    /// <summary>
    /// Interaction logic for Label.xaml
    /// </summary>
    public partial class Label : UserControl
    {
        public String ObjectType
        {
            get { return (String)this.GetValue(ObjectTypeProperty); }
            set { this.SetValue(ObjectTypeProperty, value); }
        }
        public static readonly DependencyProperty ObjectTypeProperty = DependencyProperty.Register("ObjectType", typeof(String), typeof(BasicViews.Label));

        public Label()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}
