﻿using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using LayerPlugin.ViewModels;
using LPD = LayerPlugin.Data;
using AcDb = Autodesk.AutoCAD.DatabaseServices;

namespace LayerPlugin
{
    public class LayerDataLoader
    {
        public List<LayerViewModel> GetLayers(Document document)
        {
            var documentDatabase = document.Database;

            var layerViewModels = new List<LayerViewModel>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var lt = (LayerTable)tr.GetObject(documentDatabase.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId layerId in lt)
                {
                    var acdbLayer = tr.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;

                    var layer = new LPD.Layer(acdbLayer);

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
                    var acdbCircle = (AcDb.Circle)tr.GetObject(id, OpenMode.ForRead);

                    var center = acdbCircle.Center;

                    circles.Add(new LPD.Circle
                    {
                        Id = id,
                        Center = new LPD.Coordinate(center),
                        Radius = acdbCircle.Radius,
                        LayerId = acdbCircle.LayerId
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
                    var line = (AcDb.Line)tr.GetObject(id, OpenMode.ForRead);

                    var startPoint = line.StartPoint;
                    var endPoint = line.EndPoint;

                    lines.Add(new LPD.Line
                    {
                        Id = id,
                        Start = new LPD.Coordinate(startPoint),
                        End = new LPD.Coordinate(endPoint),
                        LayerId = line.LayerId
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
                    var point = (AcDb.DBPoint)tr.GetObject(id, OpenMode.ForRead);

                    var position = point.Position;

                    points.Add(new LPD.Point
                    {
                        Id = id,
                        LayerId = point.LayerId,
                        Coordinate = new LPD.Coordinate(position)
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

            if (selRes.Status != PromptStatus.OK)
            {
                return null;
            }

            return selRes.Value.GetObjectIds(); ;
        }
    }
}
