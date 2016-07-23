using LayerPlugin.Data;

namespace LayerPlugin.Interfaces.Communicators
{
    public interface ILayerColorSelector
    {
        LayerColorSelectionResult Select(IColor color);
    }

    public struct LayerColorSelectionResult
    {
        public bool IsColorChanged { get; set; }

        public IColor NewColor { get; set; }
    }
}
