using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using LayerPlugin.Interfaces;
using LayerPlugin.ViewModels;
using LPD = LayerPlugin.Data;

namespace LayerPlugin.Helpers
{
    public class LayerDataLoader : ILayerDataLoader
    {
        private AutocadColorConverter _colorConverter;

        public LayerDataLoader()
        {
            _colorConverter = new AutocadColorConverter();
        }

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

                    var longId = layerId.OldIdPtr.ToInt64();
                    var layerColor = _colorConverter.ConvertFrom(acdbLayer.Color);

                    var layer = new LPD.Layer(longId, acdbLayer.Name, layerColor); 

                    var points = GetPointsOnLayer(acdbLayer.Name, document);
                    var circles = GetCirclesOnLayer(acdbLayer.Name, document);
                    var lines = GetLinesOnLayer(acdbLayer.Name, document);

                    var layerViewModel = new LayerViewModel(layer, points, circles, lines);
                    layerViewModels.Add(layerViewModel);
                }

            }

            return layerViewModels;
        }

        private List<LPD.Circle> GetCirclesOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var circles = new List<LPD.Circle>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "CIRCLE");
                if (ids == null)
                    return new List<LPD.Circle>();

                foreach (var id in ids)
                {
                    var acdbCircle = (Autodesk.AutoCAD.DatabaseServices.Circle)tr.GetObject(id, OpenMode.ForRead);

                    var center = acdbCircle.Center;

                    circles.Add(new LPD.Circle
                    {
                        Id = (long)id.OldIdPtr,
                        Height = acdbCircle.Thickness,
                        Center = new LPD.Coordinate(center.X, center.Y),
                        Radius = acdbCircle.Radius,

                    });
                }

                return circles;
            }
        }

        private List<LPD.Line> GetLinesOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var lines = new List<LPD.Line>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "LINE");

                if (ids == null)
                    return new List<LPD.Line>();

                foreach (var id in ids)
                {
                    var line = (Autodesk.AutoCAD.DatabaseServices.Line)tr.GetObject(id, OpenMode.ForRead);

                    var startPoint = line.StartPoint;
                    var endPoint = line.EndPoint;

                    lines.Add(new LPD.Line
                    {
                        Id = (long)id.OldIdPtr,
                        Start = new LPD.Coordinate(startPoint.X, startPoint.Y),
                        End = new LPD.Coordinate(endPoint.X, endPoint.Y),
                        Height = line.Thickness
                    });
                }

                return lines;
            }
        }

        private List<LPD.Point> GetPointsOnLayer(string layerName, Document document)
        {
            using (Transaction tr = document.Database.TransactionManager.StartOpenCloseTransaction())
            {
                var points = new List<LPD.Point>();
                var ids = GetObjectIdsInSelectedLayerOfType(document.Editor, layerName, "POINT");

                if (ids == null)
                    return new List<LPD.Point>();

                foreach (var id in ids)
                {
                    var acdbPoint = (Autodesk.AutoCAD.DatabaseServices.DBPoint)tr.GetObject(id, OpenMode.ForRead);

                    var position = acdbPoint.Position;

                    points.Add(new LPD.Point
                    {
                        Id = (long)id.OldIdPtr,
                        Coordinate = new LPD.Coordinate(position.X, position.Y),
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
