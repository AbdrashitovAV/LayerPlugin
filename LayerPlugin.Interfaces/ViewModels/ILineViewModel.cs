using LayerPlugin.Data;

namespace LayerPlugin.Interfaces.ViewModels
{
    public interface ILineViewModel: IPluginViewModel
    {
        Line Line { get; }
    }
}
