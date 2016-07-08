using LayerPlugin.Data;

namespace LayerPlugin.ViewModels
{
    public class LineViewModel: Line, IPluginViewModel
    {
        public bool IsSelected { get; }

        public LineViewModel()
        {
            
        }

        public LineViewModel(Line line)
        {
            Id = line.Id;
            Start = line.Start;
            End = line.End;
            Height = line.Height;

        }
    }
}
