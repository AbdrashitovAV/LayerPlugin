using System.Collections.Generic;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.Interfaces.Communicators
{
    public interface IStateSaver
    {
        void SaveState(IEnumerable<ILayerViewModel> layerViewModels);
    }
}