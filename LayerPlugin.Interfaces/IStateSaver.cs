using System.Collections.Generic;

namespace LayerPlugin.Interfaces
{
    public interface IStateSaver
    {
        void SaveState(IEnumerable<ILayerViewModel> layerViewModels);
    }
}