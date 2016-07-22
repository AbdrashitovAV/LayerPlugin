using System.Collections.Generic;
using System.Windows;
using LayerPlugin.Interfaces;

namespace AutocadPluginTestApp.Helpers
{
    internal class TestStateSaver : IStateSaver
    {
        public void SaveState(IEnumerable<ILayerViewModel> layerViewModels)
        {
            MessageBox.Show("Data kinda saved", "Important message");
        }
    }
}