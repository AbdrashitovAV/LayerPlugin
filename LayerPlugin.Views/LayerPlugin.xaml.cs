using System;
using System.Windows;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels;

namespace LayerPlugin.Views
{
    public partial class LayerPluginView : Window
    {
        private LayerPluginViewModel _viewModel;

        public LayerPluginView(ILayerDataLoader layerDataLoader, IStateSaver stateSaver, ILayerMoveTargetSelector layerMoveTargetSelector)
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

            _viewModel = new LayerPluginViewModel(layerDataLoader, stateSaver, layerMoveTargetSelector);

            DataContext = _viewModel;
        }
    }
}
