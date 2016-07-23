using LayerPlugin.Data;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.ViewModels
{
    public class CircleViewModel : ICircleViewModel
    {
        public bool IsSelected { get; set; }

        public Circle Circle { get; }

        public CircleViewModel()
        {
            
        }

        public CircleViewModel(Circle circle)
        {
            Circle = circle;
        }
    }
}
