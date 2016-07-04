using Autodesk.AutoCAD.DatabaseServices;

namespace LayerPlugin.Data
{
    public class AutocadObject
    {
        public ObjectId LayerId { get; set; }
        public double Height { get; set; }
    }
}
