using LayerPlugin.Data;

namespace LayerPlugin.ViewModels
{
    public class CircleViewModel : Circle, IPluginViewModel
    {
        public bool IsSelected { get; set; }

        public CircleViewModel()
        {
            
        }

        public CircleViewModel(Circle circle)
        {
            Id = circle.Id;
            Height = circle.Height;
            Radius= circle.Radius;
            Center = circle.Center;
        }
    }
}
