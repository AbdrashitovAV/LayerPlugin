using System;
using LayerPlugin.Data;
using AcCl = Autodesk.AutoCAD.Colors;

namespace LayerPlugin.Converters
{
    public class AutocadColorConverter
    {
        public Data.IColor ConvertFrom(AcCl.Color color)
        {
            if (color.ColorIndex == 7)
            {
                var first = new Data.SimpleColor(255, 255, 255);
                var second = new Data.SimpleColor(0, 0, 0);

                return new Data.ComplexColor(first, second);
            }

            var acirgb = AcCl.EntityColor.LookUpRgb((byte) color.ColorIndex);
            var b = (byte) (acirgb);
            var g = (byte) (acirgb >> 8);
            var r = (byte) (acirgb >> 16);

            return new Data.SimpleColor(r, g, b);
        }

        public AcCl.Color ConvertTo(IColor color)
        {
            if (color is SimpleColor)
            {
                var simpleColor = (SimpleColor) color;
                return AcCl.Color.FromRgb(simpleColor.Red, simpleColor.Green, simpleColor.Blue);
            }

            if (color is ComplexColor)
            {
                return AcCl.Color.FromColorIndex(AcCl.ColorMethod.ByAci, 7);
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}