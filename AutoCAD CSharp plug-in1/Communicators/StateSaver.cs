using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using LayerPlugin.Interfaces.Communicators;
using LayerPlugin.Interfaces.ViewModels;

namespace LayerPlugin.Communicators
{
    internal class StateSaver : IStateSaver
    {
        public void SaveState(IEnumerable<ILayerViewModel> layerViewModels)
        {
            var document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NoDocument, "Cannot load document");

            var documentDatabase = document.Database;

            using (Transaction transaction = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                foreach (var layerModel in layerViewModels)
                {
                    var layerId = new ObjectId(new IntPtr(layerModel.Id));

                    var layer = (LayerTableRecord) transaction.GetObject(layerId, OpenMode.ForWrite);
//                    layer.Color = layerModel.Layer.Color; //TODO: fix to normal
                    layer.Name = layerModel.Name;

//                    SaveCircles(layerModel, transaction);
//                    SaveLines(layerModel, transaction);
//                    SavePoints(layerModel, transaction);
                }

                transaction.Commit();
            }
        }
//
//        private static void SavePoints(ILayerViewModel layerModel, Transaction transaction)
//        {
//            foreach (var point in layerModel.Points)
//            {
//                var pointId = new ObjectId(new IntPtr(point.Id));
//                var databasePoint = (Autodesk.AutoCAD.DatabaseServices.DBPoint) transaction.GetObject(pointId, OpenMode.ForWrite);
//
//                databasePoint.LayerId = layerModel.Layer.Id;
//                databasePoint.Position = new Point3d(point.Coordinate.X, point.Coordinate.Y, 0);
//                databasePoint.Thickness = point.Height;
//            }
//        }
//
//        private static void SaveLines(LayerViewModel layerModel, Transaction transaction)
//        {
//            foreach (var line in layerModel.Lines)
//            {
//                var lineId = new ObjectId(new IntPtr(line.Id));
//                var databaseLine = (Autodesk.AutoCAD.DatabaseServices.Line) transaction.GetObject(lineId, OpenMode.ForWrite);
//
//                databaseLine.LayerId = layerModel.Layer.Id;
//                databaseLine.StartPoint = new Point3d(line.Start.X, line.Start.Y, 0);
//                databaseLine.EndPoint = new Point3d(line.End.X, line.End.Y, 0);
//                databaseLine.Thickness = line.Height;
//            }
//        }
//
//        private static void SaveCircles(LayerViewModel layerModel, Transaction transaction)
//        {
//            foreach (var circle in layerModel.Circles)
//            {
//                var circleId = new ObjectId(new IntPtr(circle.Id));
//                var databaseCircle = (Autodesk.AutoCAD.DatabaseServices.Circle) transaction.GetObject(circleId, OpenMode.ForWrite);
//
//                databaseCircle.LayerId = layerModel.Layer.Id;
//                databaseCircle.Center = new Point3d(circle.Center.X, circle.Center.Y, 0);
//                databaseCircle.Radius = circle.Radius;
//                databaseCircle.Thickness = circle.Height;
//            }
//        }
    }
}