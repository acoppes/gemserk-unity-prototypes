using System;
using UnityEngine;

public class Vision : MonoBehaviour {

	public float range = 100;

	[NonSerialized]
	public Vector2 cachedPosition;

	public Vector2 position
	{
		get { return transform.position; }
	}

	private TextureTest _visionSystem;
	
	public bool InRange(float x, float y) {
		return Vector2.Distance(new Vector2(x, y), transform.position) < range;
	}

	private void Start()
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
