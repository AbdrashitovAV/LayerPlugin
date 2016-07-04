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
using System.Windows.Shapes;

namespace AutoCAD_CSharp_plug_in1
{
    /// <summary>
    /// Interaction logic for LayerPlugin.xaml
    /// </summary>
    public partial class LayerPlugin : Window
    {
        private LayerPluginViewModel _viewModel;

        public LayerPlugin()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            _viewModel = new LayerPluginViewModel();

            DataContext = _viewModel;
        }
    }
}
