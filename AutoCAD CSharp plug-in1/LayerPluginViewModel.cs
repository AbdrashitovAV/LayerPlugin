using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using LayerPlugin.Views;
using Microsoft.Practices.Prism.Commands;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace LayerPlugin.ViewModels
{
    internal class LayerPluginViewModel
    {
        private List<String> _layerNames;

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

        }

        private void MoveSelected(object obj)
        {
            var sourceLayerName = (string)obj;
            var sourceLayer = LayerViewModels.Single(x => x.Layer.Name == sourceLayerName);

            var namesToShow = _layerNames.Where(x => x != sourceLayerName).ToList();

            IntPtr owner = Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.Handle;
            var modal = new TargetLayerSelectorView(namesToShow);

            Application.ShowModalWindow(owner, modal);

            var targetLayerName = modal.TargetLayer;

            if (string.IsNullOrEmpty(targetLayerName))
                return;

            var targetLayer = LayerViewModels.Single(x => x.Layer.Name == targetLayerName);
            var targetLayerId = targetLayer.Layer.Id; // TODO: make dictioary instead?

            var pointsToProcess = sourceLayer.Points.Where(x => x.IsSelected).ToList();
            foreach (var point in pointsToProcess)
            {
                point.LayerId = targetLayerId;
                sourceLayer.Points.Remove(point);
                targetLayer.Points.Add(point);
            }
            var circlesToProcess = sourceLayer.Circles.Where(x => x.IsSelected).ToList();
            foreach (var circle in circlesToProcess)
            {
                circle.LayerId = targetLayerId;
                sourceLayer.Circles.Remove(circle);
                targetLayer.Circles.Add(circle);
            }
            var linesToProcess = sourceLayer.Lines.Where(x => x.IsSelected).ToList();
            foreach (var line in linesToProcess)
            {
                line.LayerId = targetLayerId;
                sourceLayer.Lines.Remove(line);
                targetLayer.Lines.Add(line);
            }
        }


        private void CloseWindow(object obj)
        {
            var currentWindow = obj as Window;

            currentWindow?.Close();
        }

        private void ApplyAndClose(object obj)
        {
            //TODO: move logic to separate file

            var document = Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Exception(ErrorStatus.NoDocument, "Cannot load document");

            var documentDatabase = document.Database;

            using (Transaction transaction = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                foreach (var layerModel in LayerViewModels)
                {
                    var layer = transaction.GetObject(layerModel.Layer.Id, OpenMode.ForWrite) as LayerTableRecord;
                    layer.Color = layerModel.Layer.Color;
                    layer.Name = layerModel.Layer.Name;

                    foreach (var circle in layerModel.Circles)
                    {
                        var databaseCircle = (Autodesk.AutoCAD.DatabaseServices.Circle)transaction.GetObject(circle.Id, OpenMode.ForWrite);

                        databaseCircle.LayerId = layerModel.Layer.Id;
                        databaseCircle.Center = new Point3d(circle.Center.X, circle.Center.Y, 0);
                        databaseCircle.Radius = circle.Radius;
                    }

                    foreach (var line in layerModel.Lines)
                    {
                        var databaseLine = (Autodesk.AutoCAD.DatabaseServices.Line)transaction.GetObject(line.Id, OpenMode.ForWrite);

                        databaseLine.LayerId = layerModel.Layer.Id;
                        databaseLine.StartPoint = new Point3d(line.Start.X, line.Start.Y, 0);
                        databaseLine.EndPoint = new Point3d(line.End.X, line.End.Y, 0);
                    }

                    foreach (var point in layerModel.Points)
                    {
                        var databasePoint = (Autodesk.AutoCAD.DatabaseServices.DBPoint)transaction.GetObject(point.Id, OpenMode.ForWrite);

                        databasePoint.LayerId = layerModel.Layer.Id;
                        databasePoint.Position = new Point3d(point.Coordinate.X, point.Coordinate.Y, 0);
                    }
                }

                transaction.Commit();
            }


            CloseWindow(obj);
        }

        private ObservableCollection<LayerViewModel> LoadLayers()
        {
            var layerDataLoader = new LayerDataLoader();

            var document = Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Exception(ErrorStatus.NoDocument, "Cannot load document");

            return new ObservableCollection<LayerViewModel>(layerDataLoader.GetLayers(document));
        }
    }
}
