using System;
using UnityEngine;

namespace Gemserk.Vision
{
	public class Visible : MonoBehaviour
	{
		[NonSerialized] 
		public VisionPosition matrixPosition = new VisionPosition();

//	[NonSerialized]
//	public bool[] visibleByPlayer;

		[NonSerialized]
		public bool visible;

		// bounds could be selected from presets or set custom
	
		public Vector2 bounds;
	
		public Vector2 worldPosition => transform.position;

		private VisionManager _visionSystem;

		private void Awake()
		{
			_visionSystem = FindObjectOfType<VisionManager>();
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

		private void OnDrawGizmos()
		{
			// TODO: in editor, on component added auto configure bounds from sprite.
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position, bounds);
		}
	}
}
