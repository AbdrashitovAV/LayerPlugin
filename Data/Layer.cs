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

        public Layer(long id, string name, IColor color) : this()
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
