using System.Windows;
using System.Windows.Controls;
using Point  = LayerPlugin.Data.Point;
using Circle = LayerPlugin.Data.Circle;
using Line = LayerPlugin.Data.Line;

namespace LayerPlugin.Views.Helpers
{
    public class AutocadObjectDataTemplateSelector : DataTemplateSelector
    {
        DataTemplate EmptyTemplate { get; set; } = new DataTemplate();

        public DataTemplate PointTemplate { get; set; }
        public DataTemplate CircleTemplate { get; set; }
        public DataTemplate LineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return EmptyTemplate;

            //TODO: fix this !
            if ( (item as Point) != null)
                return PointTemplate;
            if ((item as Circle) != null)
                return CircleTemplate;
            if ((item as Line) != null)
                return LineTemplate;

            return EmptyTemplate;
        }
    }
}
