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

	private VisionSystem _visionSystem;

	private void Awake()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();
	}

	private void OnEnable()
	{
		if (_visionSystem != null)
			_visionSystem.Register(this);	
	}

	private void OnDisable()
	{
		if (_visionSystem != null)
			_visionSystem.Unregister(this);
	}

	public void UpdateCachedPosition()
	{
		cachedPosition = position;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, range);	
	}
	
}
