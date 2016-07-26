using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LayerPlugin.Views.BasicViews
{
    public partial class DoubleInput : UserControl
    {
        public Double Value
        {
            get { return (Double)this.GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Double), typeof(BasicViews.DoubleInput), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public String Caption
        {
            get { return (String)this.GetValue(CaptionProperty); }
            set { this.SetValue(CaptionProperty, value); }
        }
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(String), typeof(BasicViews.DoubleInput), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        public DoubleInput()
        {
            InitializeComponent();

//            (this.Content as FrameworkElement).DataContext = this;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void SetValueDP(DependencyProperty property, object value, [System.Runtime.CompilerServices.CallerMemberName] string p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
}
