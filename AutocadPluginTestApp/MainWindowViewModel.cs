using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using LayerPlugin.Data;
using LayerPlugin.ViewModels;

namespace AutocadPluginTestApp
{
    public class MainWindowViewModel
    {
        public List<Layer> Layers { get; private set; }
        public ObservableCollection<LayerViewModel> LayerViewModels { get; set;}

        public MainWindowViewModel ()
        {
            Layers = new List<Layer>();
//
//            var newLayerVM = new 
//
//            Layers.Add(new Layer { Name = "First"});
//            l
//                Items = new List<AutocadObject> {
//                    new Line() {
//                        Start = new Coordinate
//                        {
//                            X = 1, Y = 1
//                        },
//                        End = new Coordinate
//                        {
//                            X = 11, Y = 11
//                        },
//                        Height = 9
//
//                    },
//                    new Circle() {
//                        Center = new Coordinate()
//                        {
//                            X = 3, Y = 6
//                        },
//                        Height = 11
//                    },
//                    new Point() {
//                        Coordinate = new Coordinate() { X = 5, Y = 6 },
//                        Height = 10
//                    }
//                }
//            });
//            Layers.Add(new Layer { Name = "Second" });


            LayerViewModels = new ObservableCollection<LayerViewModel>(Layers.Select(layer => new LayerViewModel(layer)));
                


        }
    }

}

