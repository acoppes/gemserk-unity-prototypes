using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Vision : MonoBehaviour
{
	public int player;
	
	[FormerlySerializedAs("currentRange")]
	public float range = 100;

	public int groundLevel = 0;

	[NonSerialized]
	public VisionPosition position;
	
	[NonSerialized]
	public VisionPosition previousPosition;

	[NonSerialized]
	public float previousRange;

	[NonSerialized]
	public int previousPlayer;

	[NonSerialized]
	public int previousGroundLevel;
	
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
		previousPosition = position;
		previousRange = range;
		previousPlayer = player;
		previousGroundLevel = groundLevel;
	}

	#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = _debugColor;
		Gizmos.DrawWireSphere(worldPosition, range);	
	}
	#endif
	
}
