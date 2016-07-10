using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using LayerPlugin.Data;
using Circle = LayerPlugin.Data.Circle;

namespace LayerPlugin
{
    public class LayerDataLoader
    {
        public List<Layer> GetLayers(Document document)
        {
            var documentDatabase = document.Database;

            var lstlay = new List<Layer>();

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var lt = (LayerTable)tr.GetObject(documentDatabase.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId layerId in lt)
                {
                    var layer = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;

                    lstlay.Add(new Layer(layer));

                }

            }
            return lstlay;
        }

        private List<AutocadObject> GetObjectsOnLayer(LayerTableRecord layer, Editor ed, Transaction tr)
        {
            var objects = new List<AutocadObject>();
            TypedValue[] filterlist = new TypedValue[3] {
            /*            {

                        //filterlist[0] = new TypedValue(0, "CIRCLE,LINE,POINT");
                            new TypedValue((int)DxfCode.LayerName, "0")
                        };*/

            
            new TypedValue((int)DxfCode.Operator,  "<and"),
            new TypedValue((int)DxfCode.LayerName, @"Layer1"),
            new TypedValue((int)DxfCode.Operator,  "and>"),
            };

            var a = new TypedValue((int)DxfCode.LayerName, @"Layer1");
            var b = new TypedValue((int)DxfCode.LayerName, layer.Name);

            //filterlist.SetValue(new TypedValue((int)DxfCode.LayerName, layer.Name), 0)
            SelectionFilter filter = new SelectionFilter(filterlist);

            var selRes = ed.SelectAll(filter);



            if (selRes.Status != PromptStatus.OK)
            {
                //ed.WriteMessage("\nerror in getting the selectAll");
                return new List<AutocadObject>();
            }

            var ids = selRes.Value.GetObjectIds();


            foreach (var id in ids)
            {
                Entity ent = (Entity)tr.GetObject(id, OpenMode.ForRead);

                if (id.ObjectClass.DxfName == "CIRCLE")
                {
                    var circle = (Autodesk.AutoCAD.DatabaseServices.Circle)ent;
                    var center = circle.Center;

                    objects.Add(new Circle()
                    {
                        Center = new Coordinate(center),
                        Radius = circle.Radius
                    });
                }


            }

            return objects;

        }
    }
}
