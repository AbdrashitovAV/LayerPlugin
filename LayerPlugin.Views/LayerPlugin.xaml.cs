using System;
using System.Windows;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels;

namespace LayerPlugin.Views
{
    public partial class LayerPluginView : Window
    {
        private LayerPluginViewModel _viewModel;

        public LayerPluginView(ILayerDataLoader layerDataLoader, 
                               ILayerColorSelector layerColorSelector, 
                               ILayerMoveTargetSelector layerMoveTargetSelector, 
                               IStateSaver stateSaver)
        {
            try //TODO: remove this
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            _viewModel = new LayerPluginViewModel(layerDataLoader, layerColorSelector, layerMoveTargetSelector, stateSaver);

            DataContext = _viewModel;
        }
    }
}
