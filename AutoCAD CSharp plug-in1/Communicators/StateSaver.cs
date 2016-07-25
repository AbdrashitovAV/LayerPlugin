using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using LayerPlugin.Converters;
using LayerPlugin.Interfaces.Communicators;
using LayerPlugin.Interfaces.ViewModels;
using AcAp = Autodesk.AutoCAD.ApplicationServices;
using AcDb = Autodesk.AutoCAD.DatabaseServices;

namespace LayerPlugin.Communicators
{
    internal class StateSaver : IStateSaver
    {
        private AutocadColorConverter _colorConverter;

        public StateSaver(AutocadColorConverter autocadColorConverter)
        {
            _colorConverter = autocadColorConverter;
        }

        public void SaveState(IEnumerable<ILayerViewModel> layerViewModels)
        {
            var document = AcAp.Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.NoDocument, "Cannot load document");

            var documentDatabase = document.Database;

            using (AcDb.Transaction transaction = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                foreach (var layerModel in layerViewModels)
                {
                    var layerId = new AcDb.ObjectId(new IntPtr(layerModel.Id));

                    var layer = (AcDb.LayerTableRecord)transaction.GetObject(layerId, AcDb.OpenMode.ForWrite);
                    layer.Color = _colorConverter.ConvertTo(layerModel.Color);
                    layer.Name = layerModel.Name;

                    SavePoints(layerModel.Points, layerId, transaction);
                    SaveCircles(layerModel.Circles, layerId, transaction);
                    SaveLines(layerModel.Lines, layerId, transaction);
                }

                transaction.Commit();
            }
        }

        //TODO: make saving cool via using BaseviewModel<T> and genric method
        private void SavePoints(IList<IPointViewModel> points, AcDb.ObjectId layerId, AcDb.Transaction transaction)
        {
            foreach (var pointViewModel in points)
            {
                var point = pointViewModel.Point;

                var pointId = new AcDb.ObjectId(new IntPtr(point.Id));
                var databasePoint = (AcDb.DBPoint)transaction.GetObject(pointId, AcDb.OpenMode.ForWrite);

                databasePoint.LayerId = layerId;
                databasePoint.Position = new Point3d(point.Coordinate.X, point.Coordinate.Y, 0);
                databasePoint.Thickness = point.Height;
            }
        }

        private void SaveCircles(IList<ICircleViewModel> circleViewModels, AcDb.ObjectId layeriId, AcDb.Transaction transaction)
        {
            foreach (var circleViewModel in circleViewModels)
            {
                var circle = circleViewModel.Circle;

                var circleId = new AcDb.ObjectId(new IntPtr(circle.Id));
                var databaseCircle = (AcDb.Circle)transaction.GetObject(circleId, AcDb.OpenMode.ForWrite);

                databaseCircle.LayerId = layeriId;
                databaseCircle.Center = new Point3d(circle.Center.X, circle.Center.Y, 0);
                databaseCircle.Radius = circle.Radius;
                databaseCircle.Thickness = circle.Height;
            }
        }

        private void SaveLines(IList<ILineViewModel> lineViewModels, AcDb.ObjectId layerId, AcDb.Transaction transaction)
        {
            foreach (var lineViewModel in lineViewModels)
            {
                var line = lineViewModel.Line;

                var lineId = new AcDb.ObjectId(new IntPtr(line.Id));
                var databaseLine = (AcDb.Line)transaction.GetObject(lineId, AcDb.OpenMode.ForWrite);

                databaseLine.LayerId = layerId;
                databaseLine.StartPoint = new Point3d(line.Start.X, line.Start.Y, 0);
                databaseLine.EndPoint = new Point3d(line.End.X, line.End.Y, 0);
                databaseLine.Thickness = line.Height;
            }
        }

    }
}