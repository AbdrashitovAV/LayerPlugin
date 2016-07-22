using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using LayerPlugin.ViewModels;
using LPD = LayerPlugin.Data;
using AcDb = Autodesk.AutoCAD.DatabaseServices;

namespace LayerPlugin.Helpers
{
    internal class StateSaver
    {

        public void SaveState(Autodesk.AutoCAD.ApplicationServices.Document document, IEnumerable<LayerViewModel> layerViewModels)
        {
            var documentDatabase = document.Database;

            using (Transaction transaction = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                foreach (var layerModel in layerViewModels)
                {
                    var layer = (LayerTableRecord) transaction.GetObject(layerModel.Layer.Id, OpenMode.ForWrite);
                    layer.Color = layerModel.Layer.Color;
                    layer.Name = layerModel.Layer.Name;

                    SaveCircles(layerModel, transaction);
                    SaveLines(layerModel, transaction);
                    SavePoints(layerModel, transaction);
                }

                transaction.Commit();
            }
        }

        private static void SavePoints(LayerViewModel layerModel, Transaction transaction)
        {
            foreach (var point in layerModel.Points)
            {
                var pointId = new ObjectId(new IntPtr(point.Id));
                var databasePoint = (AcDb.DBPoint) transaction.GetObject(pointId, OpenMode.ForWrite);

                databasePoint.LayerId = layerModel.Layer.Id;
                databasePoint.Position = new Point3d(point.Coordinate.X, point.Coordinate.Y, 0);
                databasePoint.Thickness = point.Height;
            }
        }

        private static void SaveLines(LayerViewModel layerModel, Transaction transaction)
        {
            foreach (var line in layerModel.Lines)
            {
                var lineId = new ObjectId(new IntPtr(line.Id));
                var databaseLine = (AcDb.Line) transaction.GetObject(lineId, OpenMode.ForWrite);

                databaseLine.LayerId = layerModel.Layer.Id;
                databaseLine.StartPoint = new Point3d(line.Start.X, line.Start.Y, 0);
                databaseLine.EndPoint = new Point3d(line.End.X, line.End.Y, 0);
                databaseLine.Thickness = line.Height;
            }
        }

        private static void SaveCircles(LayerViewModel layerModel, Transaction transaction)
        {
            foreach (var circle in layerModel.Circles)
            {
                var circleId = new ObjectId(new IntPtr(circle.Id));
                var databaseCircle = (AcDb.Circle) transaction.GetObject(circleId, OpenMode.ForWrite);

                databaseCircle.LayerId = layerModel.Layer.Id;
                databaseCircle.Center = new Point3d(circle.Center.X, circle.Center.Y, 0);
                databaseCircle.Radius = circle.Radius;
                databaseCircle.Thickness = circle.Height;
            }
        }
    }
}