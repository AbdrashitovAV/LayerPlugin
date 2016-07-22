using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayerPlugin.Data;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels;

namespace AutocadPluginTestApp.Helpers
{
    class TestLayerDataLoader : ILayerDataLoader
    {
        public List<ILayerViewModel> GetLayers()
        {
            var layerViewModels = new List<ILayerViewModel>();

            layerViewModels.Add(StubFirstLayer());

            var layerViewModel = new LayerViewModel(new Layer(2, "Second", new SimpleColor()), new List<Point>(), new List<Circle>(), new List<Line>());
            layerViewModels.Add(layerViewModel);

            return layerViewModels;
        }

        private LayerViewModel StubFirstLayer()
        {
            var layer = new Layer(1, "First", new SimpleColor());
            var points = new List<Point>()
            {
                new Point {Coordinate = new Coordinate() {X = 5, Y = 6}, Height = 10}
            };
            var circles = new List<Circle>()
            {
                new Circle {Center = new Coordinate() {X = 3, Y = 6}, Height = 11}
            };
            var lines = new List<Line>()
            {
                new Line() {Start = new Coordinate {X = 1, Y = 1}, End = new Coordinate {X = 11, Y = 11}, Height = 9}
            };
            return new LayerViewModel(layer, points, circles, lines);
        }
    }
}
