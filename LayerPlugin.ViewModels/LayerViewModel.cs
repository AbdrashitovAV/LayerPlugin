using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using LayerPlugin.Data;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels.PropertyChanged;
using Microsoft.Practices.Prism.Commands;

namespace LayerPlugin.ViewModels
{
    public class LayerViewModel : PropertyChangedImplementation, ILayerViewModel
    {
        public Layer Layer { get; private set; }

        public System.Windows.Media.Brush ColorBrush { get; set; }

        public ObservableCollection<PointViewModel> Points { get; set; }
        public ObservableCollection<CircleViewModel> Circles { get; set; }
        public ObservableCollection<LineViewModel> Lines { get; set; }

        public DelegateCommand<object> ChangeColorCommand { get; set; }

        public LayerViewModel(Layer layer)
        {
            Points = new ObservableCollection<PointViewModel>();
            Circles  = new ObservableCollection<CircleViewModel>();
            Lines = new ObservableCollection<LineViewModel>();

            Layer = layer;
            ColorBrush = GetBrushForColor(layer.Color);

            ChangeColorCommand = new DelegateCommand<object>(OpenSelectColorDialog);
        }

        private Brush GetBrushForColor(IColor color)
        {
            if (color is SimpleColor)
            {
                var simpleColor = (SimpleColor)color;
                var solidColor = Color.FromRgb(simpleColor.Red, simpleColor.Green, simpleColor.Blue);

                return new SolidColorBrush(solidColor);
            }

            if (color is ComplexColor)
            {
                var complexColor = (ComplexColor)color;
                var firstColor = Color.FromRgb(complexColor.First.Red, complexColor.First.Green, complexColor.First.Blue);
                var secondColor = Color.FromRgb(complexColor.Second.Red, complexColor.Second.Green, complexColor.Second.Blue);

                var complexBrush = new LinearGradientBrush();
                complexBrush.GradientStops.Add(new GradientStop(firstColor, 0.0));
                complexBrush.GradientStops.Add(new GradientStop(firstColor, 0.5));
                complexBrush.GradientStops.Add(new GradientStop(secondColor, 0.51));
                complexBrush.GradientStops.Add(new GradientStop(secondColor, 1.0));

                return complexBrush;
            }

            throw new NotImplementedException();
        }

        public LayerViewModel(Layer layer, List<Point> points, List<Circle> circles, List<Line> lines) : this(layer)
        {
            Points = new ObservableCollection<PointViewModel>(points.Select(x => new PointViewModel(x)));
            Circles = new ObservableCollection<CircleViewModel>(circles.Select(x => new CircleViewModel(x)));
            Lines = new ObservableCollection<LineViewModel>(lines.Select(x => new LineViewModel(x)));
        }


        private void OpenSelectColorDialog(object obj)
        {
            //TODO: implement in dependency

            //    var colorDialog = new ColorDialog
            //    {
            //        IncludeByBlockByLayer = false,
            //        Color = Layer.Color
            //    };

            //    var dialogResult = colorDialog.ShowDialog();

            //    if (dialogResult == System.Windows.Forms.DialogResult.OK)
            //    {
            //        var selectedColor = colorDialog.Color;

            //        Layer.Color = selectedColor;
            //        ColorBrush = ColorIndexToMediaBrush(selectedColor.ColorIndex);

            //        RaisePropertyChanged(() => ColorBrush);
            //    }
        }

        //        //TODO: move this function to separate class
        //        private Brush ColorIndexToMediaBrush(int colorIndex)
        //        {
        //            if (colorIndex != 7)
        //            {
        //                var acirgb = Autodesk.AutoCAD.Colors.EntityColor.LookUpRgb((byte)colorIndex);
        //                var b = (byte)(acirgb);
        //                var g = (byte)(acirgb >> 8);
        //                var r = (byte)(acirgb >> 16);
        //
        //                var color = Color.FromRgb(r, g, b);
        //                return new SolidColorBrush(color);
        //            }
        //            else
        //            {
        //                var specialBrush = new LinearGradientBrush();
        //                specialBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
        //                specialBrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
        //                specialBrush.GradientStops.Add(new GradientStop(Colors.Black, 0.51));
        //                specialBrush.GradientStops.Add(new GradientStop(Colors.Black, 1.0));
        //                return specialBrush;
        //            }
        //        }
    }
}
