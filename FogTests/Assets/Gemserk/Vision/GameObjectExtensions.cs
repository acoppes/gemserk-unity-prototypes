using UnityEngine;

namespace Gemserk.Vision
{
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursive(this GameObject g, int layer)
        {
            g.layer = layer;
            var t = g.transform;
            var childCount = t.childCount;
            for (var i = 0; i < childCount; i++)
            {
                t.GetChild(i).gameObject.SetLayerRecursive(layer);
            }
        }	
    }
}