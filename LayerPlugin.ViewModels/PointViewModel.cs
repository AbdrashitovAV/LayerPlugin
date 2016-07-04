using LayerPlugin.Data;

namespace LayerPlugin.ViewModels
{
    public class PointViewModel: Point, IPluginViewModel
    {
        public bool IsSelected { get; }

        public PointViewModel()
        {
            
        }

        public PointViewModel(Point point)
        {
            Coordinate = point.Coordinate; //TODO: add via copy;

            Height = point.Height;
        }
    }
}
