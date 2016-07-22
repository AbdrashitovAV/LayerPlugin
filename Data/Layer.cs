namespace LayerPlugin.Data
{
    public class Layer
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public IColor Color { get; set; }

        public bool IsVisible { get; set; }


        public Layer()
        {
        }

        public Layer(int id, string name, IColor color) : this()
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }

    public interface IColor
    {
    }

    public class SimpleColor : IColor
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }

    public class ComplexColor : IColor
    {
        public SimpleColor First { get; set; }
        public SimpleColor Second { get; set; }
    }
}
