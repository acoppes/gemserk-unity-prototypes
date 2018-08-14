using UnityEngine;

namespace Gemserk.Vision
{
    public class VisionObstacleCollider2D : VisionObstacle
    {
        [SerializeField]
        protected Collider2D _collider;
    
        private void Awake()
        {
            if (_collider == null)
                _collider = GetComponentInChildren<Collider2D>();
        }

        public override short GetGroundLevel(Vector2 worldPosition)
        {
            if (_collider.OverlapPoint(worldPosition))
                return groundLevel;
            return 0;
        }
    }
}
