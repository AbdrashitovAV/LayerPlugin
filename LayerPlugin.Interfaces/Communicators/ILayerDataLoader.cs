using System.Collections.Generic;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.Interfaces.Communicators
{
    public interface ILayerDataLoader
    {
        List<ILayerViewModel> GetLayers();
    }
}