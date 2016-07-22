namespace LayerPlugin.Data
{
    public class ComplexColor : IColor
    {
        public SimpleColor First { get; set; }
        public SimpleColor Second { get; set; }
    }
}