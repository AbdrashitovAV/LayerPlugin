using System.Collections.Generic;
using System.Windows;
using LayerPlugin.Interfaces.Communicators;
using LayerPlugin.Interfaces.ViewModels;

namespace AutocadPluginTestApp.Communicators
{
    internal class TestStateSaver : IStateSaver
    {
        public void SaveState(IEnumerable<ILayerViewModel> layerViewModels)
        {
            MessageBox.Show("Data kinda saved", "Important message");
        }
    }
}