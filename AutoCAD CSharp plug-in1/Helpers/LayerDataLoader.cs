using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using LayerPlugin.Data;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels;
using Circle = LayerPlugin.Data.Circle;
using Line = LayerPlugin.Data.Line;

namespace LayerPlugin.Helpers
{
    public class LayerDataLoader : ILayerDataLoader
    {

        public List<ILayerViewModel> GetLayers()
        {
            var document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Autodesk.AutoCAD.Runtime.Exception(Autodesk.AutoCAD.Runtime.ErrorStatus.NoDocument, "Cannot load document");

            var documentDatabase = document.Database;

            var layerViewModels = new List<ILayerViewModel>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var lt = (LayerTable)tr.GetObject(documentDatabase.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId layerId in lt)
                {
                    var acdbLayer = tr.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                    //TODO: add check

                    var layer = new Data.Layer(layerId.OldIdPtr.ToInt64(), acdbLayer.Name, new Data.SimpleColor()); // TODO: fix to normal
//                    var layer = new LPD.Layer(acdbLayer);

                    var points = GetPointsOnLayer(acdbLayer.Name, document);
                    var circles = GetCirclesOnLayer(acdbLayer.Name, document);
                    var lines = GetLinesOnLayer(acdbLayer.Name, document);

                    var layerViewModel = new LayerViewModel(layer, points, circles, lines);
                    layerViewModels.Add(layerViewModel);
                }

            }

            return layerViewModels;
        }

        private List<Circle> GetCirclesOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var circles = new List<Circle>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "CIRCLE");
                if (ids == null)
                    return new List<Circle>();

                foreach (var id in ids)
                {
                    var acdbCircle = (Autodesk.AutoCAD.DatabaseServices.Circle)tr.GetObject(id, OpenMode.ForRead);

                    var center = acdbCircle.Center;

                    circles.Add(new Data.Circle
                    {
                        Id = (long)id.OldIdPtr,
                        Height = acdbCircle.Thickness,
                        Center = new Data.Coordinate(center.X, center.Y),
                        Radius = acdbCircle.Radius,

                    });
                }

                return circles;
            }
        }

        private List<Line> GetLinesOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var lines = new List<Line>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "LINE");

                if (ids == null)
                    return new List<Line>();

                foreach (var id in ids)
                {
                    var line = (Autodesk.AutoCAD.DatabaseServices.Line)tr.GetObject(id, OpenMode.ForRead);

                    var startPoint = line.StartPoint;
                    var endPoint = line.EndPoint;

                    lines.Add(new Data.Line
                    {
                        Id = (long)id.OldIdPtr,
                        Start = new Data.Coordinate(startPoint.X, startPoint.Y),
                        End = new Data.Coordinate(endPoint.X, endPoint.Y),
                        Height = line.Thickness
                    });
                }

                return lines;
            }
        }

        private List<Point> GetPointsOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var points = new List<Point>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "POINT");

                if (ids == null)
                    return new List<Point>();

                foreach (var id in ids)
                {
                    var acdbPoint = (Autodesk.AutoCAD.DatabaseServices.DBPoint)tr.GetObject(id, OpenMode.ForRead);

                    var position = acdbPoint.Position;

                    points.Add(new Data.Point
                    {
                        Id = (long)id.OldIdPtr,
                        Coordinate = new Data.Coordinate(position.X, position.Y),
                        Height = acdbPoint.Thickness
                    });
                }

                return points;
            }
        }


        private ObjectId[] GetObjectIdsInSelectedLayerOfType(Editor editor, string layerName, string objectType)
        {
            var filterlist = new TypedValue[2]
            {
                new TypedValue(0, objectType),
                new TypedValue((int) DxfCode.LayerName, layerName),
            };

            var filter = new SelectionFilter(filterlist);

            var selRes = editor.SelectAll(filter);

            return selRes.Status != PromptStatus.OK ? null : selRes.Value.GetObjectIds();
        }
    }
}
