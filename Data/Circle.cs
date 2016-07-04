using System;

namespace LayerPlugin.Data
{
    public class Circle: AutocadObject
    {
        public Coordinate Center { get; set; }

        public Double Radius { get; set; }
    }
}
