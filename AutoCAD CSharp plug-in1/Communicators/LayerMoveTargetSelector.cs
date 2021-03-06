﻿using System.Collections.Generic;
using LayerPlugin.Interfaces.Communicators;
using LayerPlugin.Views;
using AcAp = Autodesk.AutoCAD.ApplicationServices;

namespace LayerPlugin.Communicators
{
    class LayerMoveTargetSelector : ILayerMoveTargetSelector
    {
        public string Select(IEnumerable<string> namesToSelectFrom)
        {
            var selectorView = new TargetLayerSelectorView(namesToSelectFrom);

            AcAp.Application.ShowModalWindow(selectorView);

            return selectorView.TargetLayerName;
        }
    }
}
