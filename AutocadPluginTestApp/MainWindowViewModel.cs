using AutocadPluginTestApp.Helpers;
using LayerPlugin.Views;
using Microsoft.Practices.Prism.Commands;

namespace AutocadPluginTestApp
{
    public class MainWindowViewModel
    {
        public DelegateCommand<object> StartPluginCommand { get; set; }

        public MainWindowViewModel()
        { 

            StartPluginCommand = new DelegateCommand<object>(StartPlugin);
 
                


        }
        private void StartPlugin(object obj)
        {
            var layerPluginView = new LayerPluginView(new TestLayerDataLoader(),new TestLayerColorSelector(), new TestLayerMoveTargetSelector(), new TestStateSaver());

            layerPluginView.ShowDialog();
        }


    }
}

