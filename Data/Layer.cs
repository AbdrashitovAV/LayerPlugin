using System.Collections.Generic;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace LayerPlugin.Data
{
    public class Layer
    {
        private readonly LayerTableRecord _layer;

        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public Color Color { get; set; }

        public bool IsVisible { get; set; }

//        public List<AutocadObject> Items { get; set; }

        public Layer()
        {
//            Items = new List<AutocadObject>();
        }

        public Layer(LayerTableRecord layer) : this()
        {
            _layer = layer;

            Name = layer.Name;
            Color = layer.Color;
            Id = layer.Id;
        }

        public void SaveChangesToLayer()
        {
            _layer.Name = Name;
        }
    }
}
