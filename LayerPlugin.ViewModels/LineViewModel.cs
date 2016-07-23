using LayerPlugin.Data;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.ViewModels
{
    public class LineViewModel: ILineViewModel
    {
        public Line Line { get; }
        public bool IsSelected { get; set; }

        public LineViewModel()
        {
            
        }

        public LineViewModel(Line line)
        {
            Line = line;
        }
    }
}
