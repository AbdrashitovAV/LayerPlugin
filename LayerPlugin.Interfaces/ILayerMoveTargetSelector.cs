using System.Collections.Generic;

namespace LayerPlugin.Interfaces
{
    public interface ILayerMoveTargetSelector
    {
        string Select(IEnumerable<string> namesToSelectFrom);
    }
}
