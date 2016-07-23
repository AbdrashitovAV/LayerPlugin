using LayerPlugin.Data;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.ViewModels
{
    public class PointViewModel: IPointViewModel
    {
        public Point Point { get; set; }
        public bool IsSelected { get; set; }

        public PointViewModel()
        {
            
        }

        public PointViewModel(Point point)
        {
            Point = point;
        }
    }
}
