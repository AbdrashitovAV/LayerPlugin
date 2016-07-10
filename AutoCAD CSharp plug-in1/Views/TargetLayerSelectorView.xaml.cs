using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LayerPlugin.ViewModels;

namespace LayerPlugin.Views
{
    /// <summary>
    /// Interaction logic for TargetLayerSelectorView.xaml
    /// </summary>
    public partial class TargetLayerSelectorView : Window
    {
        private TargetLayerSelectorViewModel _dataContext;

        public String TargetLayer => _dataContext.SelectedLayer;

        public TargetLayerSelectorView(List<string> layers)
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
