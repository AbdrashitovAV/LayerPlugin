using System.Collections.Generic;
using LayerPlugin.Interfaces;
using LayerPlugin.Views;

namespace AutocadPluginTestApp.Helpers
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
