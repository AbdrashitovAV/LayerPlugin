using System.Collections.Generic;
using LayerPlugin.Data;

namespace LayerPlugin.Interfaces.ViewModels
{
    public interface ILayerViewModel
    {
        long Id { get; }

        string Name { get; set; }
        IColor Color { get; set; }

        System.Windows.Media.Brush ColorBrush { get; }

        IList<IPointViewModel> Points { get; set; }
        IList<ICircleViewModel> Circles { get; set; }
        IList<ILineViewModel> Lines { get; set; }

    }
}