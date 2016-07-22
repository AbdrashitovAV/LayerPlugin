namespace LayerPlugin.Data
{
    public class Layer
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public IColor Color { get; set; }

        public Layer(long id, string name, IColor color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
