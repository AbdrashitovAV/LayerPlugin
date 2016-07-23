using System.Collections.Generic;
using LayerPlugin.Interfaces.Communicators;
using LayerPlugin.Views;

namespace AutocadPluginTestApp.Communicators
{
    class TestLayerMoveTargetSelector: ILayerMoveTargetSelector
    {
        public string Select(IEnumerable<string> namesToSelectFrom)
        {
            var selectorView = new TargetLayerSelectorView(namesToSelectFrom);

            selectorView.ShowDialog();

            return selectorView.TargetLayerName;
        }
    }
}
