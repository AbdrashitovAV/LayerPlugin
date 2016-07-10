using System;
using System.Windows;
using LayerPlugin.ViewModels;

namespace LayerPlugin.Views
{
    /// <summary>
    /// Interaction logic for LayerPlugin.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LayerPluginViewModel _viewModel;

        public MainWindow()
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

            _viewModel = new LayerPluginViewModel();

            DataContext = _viewModel;
        }
    }
}
