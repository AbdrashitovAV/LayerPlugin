using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using LayerPlugin.Data;
using LayerPlugin.Interfaces.ViewModels;
using LayerPlugin.ViewModels.PropertyChanged;

namespace LayerPlugin.ViewModels
{
    public class LayerViewModel : PropertyChangedImplementation, ILayerViewModel
    {
        private readonly Layer _layer;

        public long Id => _layer.Id;

        public string Name
        {
            get { return _layer.Name; }
            set
            {
                if (_layer.Name != value)
                {
                    _layer.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        public IColor Color
        {
            get { return _layer.Color; }
            set
            {
                if (_layer.Color != value) // todo: ???
                {
                    _layer.Color = value;

                    ColorBrush = GetBrushForColor(_layer.Color);

                    RaisePropertyChanged(() => Color);
                    RaisePropertyChanged(() => ColorBrush);
                }
            }
        }

        public System.Windows.Media.Brush ColorBrush { get; private set; }

        public IList<IPointViewModel> Points { get; set; }
        public IList<ICircleViewModel> Circles { get; set; }
        public IList<ILineViewModel> Lines { get; set; }

        private Brush GetBrushForColor(IColor color)
        {
            if (color is SimpleColor)
            {
                var simpleColor = (SimpleColor)color;
                var solidColor = System.Windows.Media.Color.FromRgb(simpleColor.Red, simpleColor.Green, simpleColor.Blue);

                return new SolidColorBrush(solidColor);
            }

            if (color is ComplexColor)
            {
                var complexColor = (ComplexColor)color;
                var firstColor = System.Windows.Media.Color.FromRgb(complexColor.First.Red, complexColor.First.Green, complexColor.First.Blue);
                var secondColor = System.Windows.Media.Color.FromRgb(complexColor.Second.Red, complexColor.Second.Green, complexColor.Second.Blue);

                var complexBrush = new LinearGradientBrush();
                complexBrush.GradientStops.Add(new GradientStop(firstColor, 0.0));
                complexBrush.GradientStops.Add(new GradientStop(firstColor, 0.5));
                complexBrush.GradientStops.Add(new GradientStop(secondColor, 0.51));
                complexBrush.GradientStops.Add(new GradientStop(secondColor, 1.0));

                return complexBrush;
            }

            throw new NotImplementedException();
        }

        public LayerViewModel( Layer layer, List<Point> points, List<Circle> circles, List<Line> lines)
        {
            _layer = layer;
            ColorBrush = GetBrushForColor(layer.Color);

            Points = new ObservableCollection<IPointViewModel>(points.Select(x => new PointViewModel(x)));
            Circles = new ObservableCollection<ICircleViewModel>(circles.Select(x => new CircleViewModel(x)));
            Lines = new ObservableCollection<ILineViewModel>(lines.Select(x => new LineViewModel(x)));
        }
    }
}
