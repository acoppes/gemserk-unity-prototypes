using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gemserk.Vision
{
	public class Vision : MonoBehaviour
	{
		public int player;
	
		[FormerlySerializedAs("currentRange")]
		public float range = 100;

		public short groundLevel = 0;

		[NonSerialized]
		public VisionPosition position;
	
#if UNITY_EDITOR
		public Color _debugColor;
#endif
	
		public Vector2 worldPosition
		{
			get { return transform.position; }
		}

		private VisionManager _visionSystem;

		private void Awake()
		{
			_visionSystem = FindObjectOfType<VisionManager>();
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

#if UNITY_EDITOR
		void OnDrawGizmos() {
			Gizmos.color = _debugColor;
			Gizmos.DrawWireSphere(worldPosition, range);	
		}
#endif
	
	}
}
