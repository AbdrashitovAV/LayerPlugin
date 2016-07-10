﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using LayerPlugin.Data;
using LayerPlugin.Views;
using Microsoft.Practices.Prism.Commands;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = Autodesk.AutoCAD.Runtime.Exception;
using LPD = LayerPlugin.Data;

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

            LayerViewModels = new ObservableCollection<LayerViewModel>();
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

            var layers = layerDataLoader.GetLayers(document);
            var layerViewModels = new ObservableCollection<LayerViewModel>();

            var circles = LoadAllCircles(document);
            var lines = LoadAllLines(document);
            var points = LoadAllPoints(document);

            foreach (var layer in layers)
            {
                var layerViewModel = new LayerViewModel(layer)
                {
                    Circles = new ObservableCollection<CircleViewModel>(
                        circles.
                        Where(x => x.LayerId == layer.Id).
                        Select(x => new CircleViewModel(x)).
                        ToList()),
                    Points = new ObservableCollection<PointViewModel>(
                        points.
                        Where(x => x.LayerId == layer.Id).
                        Select(x => new PointViewModel(x)).
                        ToList()),
                    Lines = new ObservableCollection<LineViewModel>(
                        lines.
                        Where(x => x.LayerId == layer.Id).
                        Select(x => new LineViewModel(x)).
                        ToList())
                };



                layerViewModels.Add(layerViewModel);
            }
            return layerViewModels;
        }


        private List<LPD.Circle> LoadAllCircles(Document document)
        {
            var documentDatabase = document.Database;
            var editor = document.Editor;
            var circles = new List<LPD.Circle>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var filterlist = new TypedValue[1]
                {
                    new TypedValue(0, "CIRCLE")
                };

                var filter = new SelectionFilter(filterlist);
                var selectionResult = editor.SelectAll(filter);
                if (selectionResult.Status != PromptStatus.OK)
                {
                    return new List<LPD.Circle>();
                }
                var ids = selectionResult.Value.GetObjectIds();

                foreach (var id in ids)
                {
                    var circle = (Autodesk.AutoCAD.DatabaseServices.Circle)tr.GetObject(id, OpenMode.ForRead);

                    var center = circle.Center;

                    circles.Add(new LPD.Circle
                    {
                        Id = id,
                        Center = new Coordinate(center),
                        Radius = circle.Radius,
                        LayerId = circle.LayerId
                    });
                }
            }

            return circles;
        }


        private List<LPD.Line> LoadAllLines(Document document)
        {
            var documentDatabase = document.Database;
            var editor = document.Editor;
            var lines = new List<LPD.Line>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var filterlist = new TypedValue[1]
                {
                    new TypedValue(0, "LINE")
                };

                var filter = new SelectionFilter(filterlist);
                var selectionResult = editor.SelectAll(filter);

                if (selectionResult.Status != PromptStatus.OK)
                {
                    return new List<LPD.Line>();
                }
                var ids = selectionResult.Value.GetObjectIds();

                foreach (var id in ids)
                {
                    var line = (Autodesk.AutoCAD.DatabaseServices.Line)tr.GetObject(id, OpenMode.ForRead);

                    var startPoint = line.StartPoint;
                    var endPoint = line.EndPoint;

                    lines.Add(new LPD.Line
                    {
                        Id = id,
                        Start = new Coordinate(startPoint),
                        End = new Coordinate(endPoint),
                        LayerId = line.LayerId
                    });
                }
            }

            return lines;
        }

        private List<LPD.Point> LoadAllPoints(Document document)
        {
            var documentDatabase = document.Database;
            var editor = document.Editor;
            var points = new List<LPD.Point>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var filterlist = new TypedValue[1]
                {
                    new TypedValue(0, "POINT")
                };

                var filter = new SelectionFilter(filterlist);
                var selectionResult = editor.SelectAll(filter);

                if (selectionResult.Status != PromptStatus.OK)
                {
                    return new List<LPD.Point>();
                }

                var ids = selectionResult.Value.GetObjectIds();

                foreach (var id in ids)
                {
                    var point = (Autodesk.AutoCAD.DatabaseServices.DBPoint)tr.GetObject(id, OpenMode.ForRead);

                    var position = point.Position;

                    points.Add(new LPD.Point
                    {
                        Id = id,
                        LayerId = point.LayerId,
                        Coordinate = new Coordinate(position)
                    });
                }
            }

            return points;
        }
    }
}
