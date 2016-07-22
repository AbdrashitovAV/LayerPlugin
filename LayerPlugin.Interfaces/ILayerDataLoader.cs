using System.Collections.Generic;

namespace LayerPlugin.Interfaces
{
    public interface ILayerDataLoader
    {
        List<ILayerViewModel> GetLayers();
    }
}