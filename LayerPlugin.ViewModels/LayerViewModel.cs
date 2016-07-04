using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Autodesk.AutoCAD.Windows;
using LayerPlugin.Data;
using LayerPlugin.ViewModels.PropertyChanged;
using Microsoft.Practices.Prism.Commands;

namespace LayerPlugin.ViewModels
{
    public class LayerViewModel : PropertyChangedImplementation
    {
        public Layer Layer { get; private set; }

        public System.Windows.Media.Brush ColorBrush { get; set; }
        public ObservableCollection<AutocadObject> Items { get; set; } // todo: switch type

        public DelegateCommand<object> ChangeColorCommand { get; set; }

        public LayerViewModel(Layer layer)
        {
            Layer = layer;
            ColorBrush = ColorIndexToMediaBrush(layer.Color.ColorIndex);
            //            layer.Color.
            //
            //            Items = new ObservableCollection<AutocadObject>(layer.Items.Select(x =>
            //            {
            //                if (x.GetType() == typeof(Point))
            //                    return  new PointViewModel(x as Point);
            //
            //                if (x.GetType() == typeof(Line))
            //                    return new LineViewModel(x as Line);
            //
            //                if (x.GetType() == typeof(Circle))
            //                    return new CircleViewModel(x as Circle);
            //
            //                return x;
            //            }));

            ChangeColorCommand = new DelegateCommand<object>(OpenSelectColorDialog);
        }


        private void OpenSelectColorDialog(object obj)
        {

            var colorDialog = new ColorDialog
            {
                IncludeByBlockByLayer = false,
                Color = Layer.Color
            };

            var dialogResult = colorDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                var selectedColor = colorDialog.Color;

                Layer.Color = selectedColor;
                ColorBrush = ColorIndexToMediaBrush(selectedColor.ColorIndex);

                RaisePropertyChanged(() => ColorBrush);
            }
        }

        //TODO: move this function to separate class
        private Brush ColorIndexToMediaBrush(int colorIndex)
        {
            if (colorIndex != 7)
            {
                var acirgb = Autodesk.AutoCAD.Colors.EntityColor.LookUpRgb((byte)colorIndex);
                var b = (byte)(acirgb);
                var g = (byte)(acirgb >> 8);
                var r = (byte)(acirgb >> 16);

                var color = Color.FromRgb(r, g, b);
                return new SolidColorBrush(color);
            }
            else
            {
                var specialBrush = new LinearGradientBrush();
                specialBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                specialBrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                specialBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.51));
                specialBrush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
                return specialBrush;
            }
        }
    }
}
