using System.Collections.Generic;
using System.Windows;
using LayerPlugin.ViewModels;

namespace LayerPlugin.Views
{
    public partial class TargetLayerSelectorView : Window
    {
        private TargetLayerSelectorViewModel _dataContext;

        public string TargetLayerName => _dataContext.SelectedLayer;

        public TargetLayerSelectorView(IEnumerable<string> layers)
        {
            InitializeComponent();

            _dataContext = new TargetLayerSelectorViewModel(layers, ProcessWindowClosing);

            DataContext = _dataContext;
        }

        private void ProcessWindowClosing(bool shouldSaveSelectedLayer)
        {
            if (!shouldSaveSelectedLayer)
                _dataContext.SelectedLayer = null;

            Close();
        }
    }
}
