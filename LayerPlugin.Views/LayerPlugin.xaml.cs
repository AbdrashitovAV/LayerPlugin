using System.Windows;
using LayerPlugin.Interfaces.Communicators;
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
            InitializeComponent();

            _viewModel = new LayerPluginViewModel(layerDataLoader, layerColorSelector, layerMoveTargetSelector, stateSaver);

            DataContext = _viewModel;
        }
    }
}
