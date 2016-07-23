using Autodesk.AutoCAD.Windows;
using LayerPlugin.Converters;
using LayerPlugin.Data;
using LayerPlugin.Interfaces.Communicators;

namespace LayerPlugin.Communicators
{
    class AutocadLayerColorSelector : ILayerColorSelector
    {
        private AutocadColorConverter _colorConverter;

        public AutocadLayerColorSelector()
        {
            //TODO: to dependency
            _colorConverter = new AutocadColorConverter();
        }

        public LayerColorSelectionResult Select(IColor color)
        {
            var colorDialog = new ColorDialog
            {
                IncludeByBlockByLayer = false,
                Color = _colorConverter.ConvertTo(color)
            };

            var dialogResult = colorDialog.ShowDialog();

            if (dialogResult != System.Windows.Forms.DialogResult.OK)
                return new LayerColorSelectionResult { IsColorChanged = false };

            var selectedColor = colorDialog.Color;

            return new LayerColorSelectionResult { IsColorChanged = true, NewColor = _colorConverter.ConvertFrom(selectedColor) };
        }
    }
}

