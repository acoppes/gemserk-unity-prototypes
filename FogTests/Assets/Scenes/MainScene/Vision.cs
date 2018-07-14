using System;
using UnityEngine;

public class Vision : MonoBehaviour {

	public float range = 100;

	[NonSerialized]
	public int[] matrixPosition = new int[2];
	
	[NonSerialized]
	public int[] cachedPosition = new int[2];

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
		cachedPosition[0] = matrixPosition[0];
		cachedPosition[1] = matrixPosition[1];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, range);	
	}
	
}
