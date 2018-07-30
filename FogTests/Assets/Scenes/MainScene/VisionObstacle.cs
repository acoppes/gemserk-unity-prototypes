using UnityEngine;

public class VisionObstacle : MonoBehaviour
{
    public short groundLevel;
    
//    private VisionSystem _visionSystem;

    [SerializeField]
    protected Collider2D _collider;
    
    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponentInChildren<Collider2D>();
//        _visionSystem = FindObjectOfType<VisionSystem>();
    }

//    private void OnEnable()
//    {
//        if (_visionSystem != null)
//            _visionSystem.RegisterObstacle(this);	
//    }
//
//    private void OnDisable()
//    {
//        if (_visionSystem != null)
//            _visionSystem.UnregisterObstacle(this);
//    }

    public short GetGroundLevel(Vector2 worldPosition)
    {
        if (_collider.OverlapPoint(worldPosition))
            return groundLevel;
        return 0;
    }
}
