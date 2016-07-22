using LayerPlugin.Data;

namespace LayerPlugin.Interfaces
{
    public interface ILayerViewModel
    {
        long Id { get; }

        string Name { get; set; }
        IColor Color { get; set; }

        System.Windows.Media.Brush ColorBrush { get; }

//        public ObservableCollection<PointViewModel> Points { get; set; }
//        public ObservableCollection<CircleViewModel> Circles { get; set; }
//        public ObservableCollection<LineViewModel> Lines { get; set; }

    }
}