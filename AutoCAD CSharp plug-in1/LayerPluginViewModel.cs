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
using LayerPlugin.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Circle = LayerPlugin.Data.Circle;
using Point = LayerPlugin.Data.Point;

namespace AutoCAD_CSharp_plug_in1
{
    internal class LayerPluginViewModel
    {
        public ObservableCollection<LayerViewModel> LayerViewModels { get; set; }

        public DelegateCommand<object> CloseCommand { get; set; }
        public DelegateCommand<object> ApplyAndCloseCommand { get; set; }


        public LayerPluginViewModel()
        {
            CloseCommand = new DelegateCommand<object>(CloseWindow);
            ApplyAndCloseCommand = new DelegateCommand<object>(ApplyAndClose);

            LayerViewModels = new ObservableCollection<LayerViewModel>();

            LoadLayers();
        }


        private void CloseWindow(object obj)
        {
            var currentWindow = obj as Window;

            currentWindow?.Close();
        }

        private void ApplyAndClose(object obj)
        {
            //TODO: move to separate file

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

                    foreach (var item in layerModel.Items)
                    {
                        if ((item as Circle) != null)
                        {
                            var circle = (Circle) item;
                            var databaseCircle = (Autodesk.AutoCAD.DatabaseServices.Circle) transaction.GetObject(layerModel.Layer.Id, OpenMode.ForWrite);

                            databaseCircle.LayerId = layerModel.Layer.Id;
                            databaseCircle.Center = new Point3d(circle.Center.X, circle.Center.Y, 0);
                            databaseCircle.Radius = circle.Radius;
                        }
                    }
                }

                transaction.Commit();
            }


            CloseWindow(obj);
        }


        private void LoadLayers()
        {
            var layerDataLoader = new LayerDataLoader();

            var document = Application.DocumentManager.MdiActiveDocument;

            if (document == null)
                throw new Exception(ErrorStatus.NoDocument, "Cannot load document");

            var layers = layerDataLoader.GetLayers(document);

            var objects = LoadAllObjects(document);

            foreach (var layer in layers)
            {
                var layerViewModel = new LayerViewModel(layer);

                layerViewModel.Items = new ObservableCollection<AutocadObject>(objects.Where(x => x.LayerId == layer.Id).ToList());
                LayerViewModels.Add(layerViewModel);
            }

            //                ed.WriteMessage("Hello, this is your first command.");
        }

        private List<AutocadObject> LoadAllObjects(Document document)
        {
            var documentDatabase = document.Database;
            var editor = document.Editor;

            using (Transaction tr = documentDatabase.TransactionManager.StartOpenCloseTransaction())
            {
                var objects = new List<AutocadObject>();
                var filterlist = new TypedValue[1]
                {
                    new TypedValue(0, "CIRCLE,LINE,POINT")
                };


                var filter = new SelectionFilter(filterlist);

                var selectionResult = editor.SelectAll(filter);

                var ids = selectionResult.Value.GetObjectIds();

                foreach (var id in ids)
                {
                    var entity = (Entity)tr.GetObject(id, OpenMode.ForRead);

                    if (id.ObjectClass.DxfName == "CIRCLE")
                    {
                        var circle = ((Autodesk.AutoCAD.DatabaseServices.Circle)entity);
                        var center = circle.Center;

                        objects.Add(new Circle()
                        {
                            Center = new Coordinate(center.X, center.Y),
                            Radius = circle.Radius,
                            LayerId = circle.LayerId
                        });
                    }
                }

                return objects;
            }
        }
    }
}
