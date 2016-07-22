using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using LayerPlugin.Interfaces;
using Microsoft.Practices.Prism.Commands;
//using AcAp = Autodesk.AutoCAD.ApplicationServices;
//using AcDb = Autodesk.AutoCAD.DatabaseServices;
//using AcRx = Autodesk.AutoCAD.Runtime;

namespace LayerPlugin.ViewModels
{
    public class LayerPluginViewModel
    {
        private readonly ILayerDataLoader _layerDataLoader;
        private readonly ILayerColorSelector _layerColorSelector;
        private readonly IStateSaver _stateSaver;
        private readonly ILayerMoveTargetSelector _layerMoveTargetSelector;
        private List<String> _layerNames;

        public ObservableCollection<LayerViewModel> LayerViewModels { get; set; }

        public DelegateCommand<object> ChangeLayerColorCommand { get; set; }
        public DelegateCommand<object> MoveSelectedCommand { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }
        public DelegateCommand<object> ApplyAndCloseCommand { get; set; }


        public LayerPluginViewModel(ILayerDataLoader layerDataLoader,
                                     ILayerColorSelector layerColorSelector,
                                    ILayerMoveTargetSelector layerMoveTargetSelector,
                                    IStateSaver stateSaver )
        {
            _layerDataLoader = layerDataLoader;
            _layerColorSelector = layerColorSelector;
            _layerMoveTargetSelector = layerMoveTargetSelector;
            _stateSaver = stateSaver;

            ChangeLayerColorCommand = new DelegateCommand<object>(ChangeLayerColor);
            MoveSelectedCommand = new DelegateCommand<object>(MoveSelected);
            CloseCommand = new DelegateCommand<object>(CloseWindow);
            ApplyAndCloseCommand = new DelegateCommand<object>(ApplyAndClose);

            LayerViewModels = LoadLayers();
            _layerNames = LayerViewModels.Select(x => x.Name).ToList();
        }

        private void ChangeLayerColor(object obj)
        {
            var id = (long)obj;

            var layer = LayerViewModels.Single(x => x.Id == id);

            var selectionResult = _layerColorSelector.Select(layer.Color);

            if(!selectionResult.IsColorChanged)
                return;

            layer.Color = selectionResult.NewColor;
        }

        private ObservableCollection<LayerViewModel> LoadLayers()
        {
            return new ObservableCollection<LayerViewModel>(_layerDataLoader.GetLayers().Select(x => (LayerViewModel)x)); //TODO: I don't like this select
        }

        #region Move objects
        private void MoveSelected(object obj)
        {
            var sourceLayerName = (string)obj;

            var targetLayerName = GetTargetLayerName(sourceLayerName);

            if (string.IsNullOrEmpty(targetLayerName))
                return;

            var sourceLayer = LayerViewModels.Single(x => x.Name == sourceLayerName);
            var targetLayer = LayerViewModels.Single(x => x.Name == targetLayerName);

            MovePointsToAnotherLayer(sourceLayer, targetLayer);
            MoveCirclesToAnotherLayer(sourceLayer, targetLayer);
            MoveLinesToAnotherLayer(sourceLayer, targetLayer);
        }

        private string GetTargetLayerName(string sourceLayerName)
        {

            var namesToShow = _layerNames.Where(x => x != sourceLayerName).ToList();

            //            var modal = new TargetLayerSelectorView(namesToShow);
            //
            //            AcAp.Application.ShowModalWindow(modal);
            //
            //            var targetLayerName = modal.TargetLayer;

            var targetLayerName = _layerMoveTargetSelector.Select(namesToShow);

            return targetLayerName;
        }

        private static void MoveLinesToAnotherLayer(LayerViewModel sourceLayer, LayerViewModel targetLayer)
        {
            var linesToProcess = sourceLayer.Lines.Where(x => x.IsSelected).ToList();
            foreach (var line in linesToProcess)
            {
                line.IsSelected = false;
                targetLayer.Lines.Add(line);
                sourceLayer.Lines.Remove(line);
            }
        }

        private static void MoveCirclesToAnotherLayer(LayerViewModel sourceLayer, LayerViewModel targetLayer)
        {
            var circlesToProcess = sourceLayer.Circles.Where(x => x.IsSelected).ToList();
            foreach (var circle in circlesToProcess)
            {
                circle.IsSelected = false;
                targetLayer.Circles.Add(circle);
                sourceLayer.Circles.Remove(circle);
            }
        }

        private static void MovePointsToAnotherLayer(LayerViewModel sourceLayer, LayerViewModel targetLayer)
        {
            var pointsToProcess = sourceLayer.Points.Where(x => x.IsSelected).ToList();
            foreach (var point in pointsToProcess)
            {
                point.IsSelected = false;
                targetLayer.Points.Add(point);
                sourceLayer.Points.Remove(point);
            }
        }
        #endregion

        private void ApplyAndClose(object obj)
        {
            _stateSaver.SaveState(LayerViewModels);

            CloseWindow(obj);
        }

        private void CloseWindow(object obj)
        {
            var currentWindow = obj as System.Windows.Window;

            currentWindow?.Close();
        }
    }
}
