using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Vision
{
	public class VisionManager : MonoBehaviour
	{
		[SerializeField]
		protected VisionSystem _visionSystem;
			
		private readonly List<Vision> _visions = new List<Vision>();

		private readonly List<Vision> _addedVisions = new List<Vision>();
		private readonly List<Vision> _removedVisions = new List<Vision>();
		
		private bool _dirty;
		
		private readonly List<Visible> _visibles = new List<Visible>();
		
		private void ProcessPendingVisions()
		{
			foreach (var vision in _addedVisions)
			{
				_visions.Add(vision);
				_dirty = true;
			}

			_addedVisions.Clear();

			foreach (var vision in _removedVisions)
			{
				_visions.Remove(vision);
				_dirty = true;
			}

			_removedVisions.Clear();
		}

		public void Register(Vision vision)
		{
			_addedVisions.Add(vision);
		}

		public void Unregister(Vision vision)
		{
			_removedVisions.Add(vision);
		}

		public void AddVisible(Visible visible)
		{
			_visibles.Add(visible);
		}

		public void RemoveVisible(Visible visible)
		{
			_visibles.Remove(visible);
		}
		
		private void FixedUpdate()
		{
			ProcessPendingVisions();
			
			_visionSystem.UpdateVision(_visions);
			_visionSystem.UpdateVisibles(_visibles);
			
			_visionSystem.UpdateTextures();
		}

	}
}
