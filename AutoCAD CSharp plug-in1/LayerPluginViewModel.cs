using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using LayerPlugin.Helpers;
using LayerPlugin.Views;
using Microsoft.Practices.Prism.Commands;
using AcAp = Autodesk.AutoCAD.ApplicationServices;
using AcDb = Autodesk.AutoCAD.DatabaseServices;
using AcRx = Autodesk.AutoCAD.Runtime;

namespace LayerPlugin.ViewModels
{
    internal class LayerPluginViewModel
    {
        private List<String> _layerNames;
        private readonly StateSaver _stateSaver;

        public ObservableCollection<LayerViewModel> LayerViewModels { get; set; }

        public DelegateCommand<object> MoveSelectedCommand { get; set; }
        public DelegateCommand<object> CloseCommand { get; set; }
        public DelegateCommand<object> ApplyAndCloseCommand { get; set; }


        public LayerPluginViewModel()
        {
            MoveSelectedCommand = new DelegateCommand<object>(MoveSelected);
            CloseCommand = new DelegateCommand<object>(CloseWindow);
            ApplyAndCloseCommand = new DelegateCommand<object>(ApplyAndClose);

            LayerViewModels = LoadLayers();
            _layerNames = LayerViewModels.Select(x => x.Layer.Name).ToList();
            _stateSaver = new StateSaver();
        }

        private ObservableCollection<LayerViewModel> LoadLayers()
        {
            var layerDataLoader = new LayerDataLoader();

            var document = AcAp.Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new AcRx.Exception(ErrorStatus.NoDocument, "Cannot load document");

            return new ObservableCollection<LayerViewModel>(layerDataLoader.GetLayers(document));
        }

        #region move objects
        private void MoveSelected(object obj)
        {
            var sourceLayerName = (string)obj;

            var targetLayerName = GetTargetLayerName(sourceLayerName);

            if (string.IsNullOrEmpty(targetLayerName))
                return;

            var sourceLayer = LayerViewModels.Single(x => x.Layer.Name == sourceLayerName);
            var targetLayer = LayerViewModels.Single(x => x.Layer.Name == targetLayerName);
            var targetLayerId = targetLayer.Layer.Id; // TODO: make dictioary instead?

            MovePointsToAnotherLayer(sourceLayer, targetLayerId, targetLayer);
            MoveCirclesToAnotherLayer(sourceLayer, targetLayerId, targetLayer);
            MoveLinesToAnotherLayer(sourceLayer, targetLayerId, targetLayer);
        }

        private string GetTargetLayerName(string sourceLayerName)
        {
            var namesToShow = _layerNames.Where(x => x != sourceLayerName).ToList();
            var modal = new TargetLayerSelectorView(namesToShow);

            AcAp.Application.ShowModalWindow(modal);

            var targetLayerName = modal.TargetLayer;
            return targetLayerName;
        }

        private static void MoveLinesToAnotherLayer(LayerViewModel sourceLayer, ObjectId targetLayerId, LayerViewModel targetLayer)
        {
            var linesToProcess = sourceLayer.Lines.Where(x => x.IsSelected).ToList();
            foreach (var line in linesToProcess)
            {
                line.IsSelected = false;
                sourceLayer.Lines.Remove(line);
                targetLayer.Lines.Add(line);
            }
        }

        private static void MoveCirclesToAnotherLayer(LayerViewModel sourceLayer, ObjectId targetLayerId, LayerViewModel targetLayer)
        {
            var circlesToProcess = sourceLayer.Circles.Where(x => x.IsSelected).ToList();
            foreach (var circle in circlesToProcess)
            {
                circle.IsSelected = false;
                sourceLayer.Circles.Remove(circle);
                targetLayer.Circles.Add(circle);
            }
        }

        private static void MovePointsToAnotherLayer(LayerViewModel sourceLayer, ObjectId targetLayerId, LayerViewModel targetLayer)
        {
            var pointsToProcess = sourceLayer.Points.Where(x => x.IsSelected).ToList();
            foreach (var point in pointsToProcess)
            {
                point.IsSelected = false;
                sourceLayer.Points.Remove(point);
                targetLayer.Points.Add(point);
            }
        }
        #endregion

        private void ApplyAndClose(object obj)
        {
            //TODO: move logic to separate file

            var document = AcAp.Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new AcRx.Exception(ErrorStatus.NoDocument, "Cannot load document");

            _stateSaver.SaveState(document, LayerViewModels);
            
            CloseWindow(obj);
        }

        private void CloseWindow(object obj)
        {
            var currentWindow = obj as Window;

            currentWindow?.Close();
        }
    }
}
