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
	
	public Vector2 worldPosition => transform.position;

	private VisionSystem _visionSystem;

	private void Awake()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();
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
