using UnityEngine;

namespace Gemserk.Vision
{
    public abstract class VisionObstacle : MonoBehaviour
    {
        public short groundLevel;
        public abstract short GetGroundLevel(Vector2 worldPosition);
    }
}