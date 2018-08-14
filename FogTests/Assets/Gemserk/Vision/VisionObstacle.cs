using UnityEngine;

public class VisionObstacle : MonoBehaviour
{
    public short groundLevel;
    
    [SerializeField]
    protected Collider2D _collider;
    
    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponentInChildren<Collider2D>();
    }

    public short GetGroundLevel(Vector2 worldPosition)
    {
        if (_collider.OverlapPoint(worldPosition))
            return groundLevel;
        return 0;
    }
}
