namespace LayerPlugin.Data
{
    public class SimpleColor : IColor
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public SimpleColor()
        {
            
        }

        public SimpleColor(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
}