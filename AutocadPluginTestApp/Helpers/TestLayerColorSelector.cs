using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LayerPlugin.Data;
using LayerPlugin.Interfaces;

namespace AutocadPluginTestApp.Helpers
{
    class TestLayerColorSelector : ILayerColorSelector
    {
        private Random _random;

        public TestLayerColorSelector()
        {
            _random = new Random();
        }

        public LayerColorSelectionResult Select(IColor color)
        {
            var colorBuffer = new byte[3];
            _random.NextBytes(colorBuffer);

            return new LayerColorSelectionResult() { IsColorChanged = true, NewColor = new SimpleColor(colorBuffer[0], colorBuffer[1], colorBuffer[2]) };
        }
    }
}
