using LayerPlugin.Data;

namespace LayerPlugin.ViewModels
{
    public class PointViewModel: Point, IPluginViewModel
    {
        public bool IsSelected { get; set; }

        public PointViewModel()
        {
            
        }

        public PointViewModel(Point point)
        {
            Id = point.Id;

            Coordinate = point.Coordinate; //TODO: add via copy?

            Height = point.Height;
        }
    }
}
