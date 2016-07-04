using LayerPlugin.Data;

namespace LayerPlugin.ViewModels
{
    public class CircleViewModel : Circle, IPluginViewModel
    {
        public bool IsSelected { get; }

        public CircleViewModel()
        {
            
        }

        public CircleViewModel(Circle circle)
        {
            Height = circle.Height;
            Radius= circle.Radius;
            Center = circle.Center;
        }
    }
}
