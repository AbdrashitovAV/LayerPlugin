using System.Collections.Generic;

namespace LayerPlugin.Interfaces.Communicators
{
    public interface ILayerMoveTargetSelector
    {
        string Select(IEnumerable<string> namesToSelectFrom);
    }
}
