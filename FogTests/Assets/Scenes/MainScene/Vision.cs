using UnityEngine;

public interface IVision
{
	bool InRange(float x, float y);
}

public class Vision : MonoBehaviour, IVision {

	public float range = 100;

	private TextureTest _visionSystem;
	
	public bool InRange(float x, float y) {
		return Vector2.Distance(new Vector2(x, y), transform.position) < range;
	}

	private void Awake()
	{
		_visionSystem = FindObjectOfType<TextureTest>();
		if (_visionSystem != null)
			_visionSystem.Register(this);
	}

	private void OnDestroy()
	{
		if (_visionSystem != null)
			_visionSystem.Unregister(this);
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, range);	
	}
	
}
