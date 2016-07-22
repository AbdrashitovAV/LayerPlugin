using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;

namespace LayerPlugin.ViewModels
{
    public class TargetLayerSelectorViewModel
    {
        private readonly Action<bool> _onCloseCallback;

        public IEnumerable<string> Layers { get; set; }
        public string SelectedLayer { get; set; }

        public DelegateCommand<object> CloseWindowCommand { get; set; }

        public TargetLayerSelectorViewModel(IEnumerable<string> layers, Action<bool> onCloseCallback)
        {
            _onCloseCallback = onCloseCallback;
            Layers = layers;

            CloseWindowCommand = new DelegateCommand<object>(CloseWindow);
        }

        private void CloseWindow(object obj)
        {
            if (obj == null)
                return;
            var shouldSaveSelectedLayer = (bool)obj;

            _onCloseCallback(shouldSaveSelectedLayer);
        }
    }
}
