using System;
using UnityEngine;

public class Vision : MonoBehaviour {

	public float currentRange = 100;

	[NonSerialized]
	public VisionPosition currentPosition;
	
	[NonSerialized]
	public VisionPosition previousPosition;

	[NonSerialized]
	public float previousRange;
	
	#if UNITY_EDITOR
	public Color _debugColor;
	#endif
	
	public Vector2 worldPosition
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
		previousPosition = currentPosition;
		previousRange = currentRange;
	}

	#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = _debugColor;
		Gizmos.DrawWireSphere(worldPosition, currentRange);	
	}
	#endif
	
}
