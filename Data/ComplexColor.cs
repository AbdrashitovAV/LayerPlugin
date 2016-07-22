namespace LayerPlugin.Data
{
    public class ComplexColor : IColor
    {
        public SimpleColor First { get; set; }
        public SimpleColor Second { get; set; }

        public ComplexColor(SimpleColor first, SimpleColor second)
        {
            First = first;
            Second = second;
        }
    }
}