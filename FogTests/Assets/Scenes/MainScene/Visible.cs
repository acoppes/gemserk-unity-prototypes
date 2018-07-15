using System;
using UnityEngine;

public class Visible : MonoBehaviour
{
	[NonSerialized]
	public readonly int[] matrixPosition = new int[2];

//	[NonSerialized]
//	public bool[] visibleByPlayer;

	[NonSerialized]
	public bool visible;

	public Bounds bounds;
	
	public Vector2 worldPosition => transform.position;

	private VisionSystem _visionSystem;

	private void Awake()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();

		var collider = GetComponent<BoxCollider2D>();
		if (collider != null)
		{
			bounds.size = GetComponent<BoxCollider2D>().bounds.size;
			collider.enabled = false;
		}
	}
	
	private void OnEnable()
	{
		if (_visionSystem != null)
			_visionSystem.AddVisible(this);	
	}

	private void OnDisable()
	{
		if (_visionSystem != null)
			_visionSystem.RemoveVisible(this);
	}

}
