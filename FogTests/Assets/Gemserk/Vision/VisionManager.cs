using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

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
		
		[SerializeField]
		protected float _updateTotal;
		
		private float _updateCurrent;

		private void Start()
		{
			_visionSystem.Init();
			
			var obstacles = FindObjectsOfType<VisionObstacle>();
			foreach (var obstacle in obstacles)
			{
				_visionSystem.RegisterObstacle(obstacle);
			}
			
			_updateCurrent = _updateTotal;
		}
		
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
			
			_updateCurrent += Time.deltaTime;

			if (_updateCurrent < _updateTotal)
				return;
			
			Profiler.BeginSample("VisionUpdate");
				
			_visionSystem.ClearVision();
			foreach (var vision in _visions)
			{
				_visionSystem.UpdateVision(vision);
			}
				
			Profiler.EndSample();
				
							
			Profiler.BeginSample("Visible");
			for (var i = 0; i < _visibles.Count; i++)
			{
				var visible = _visibles[i];
				_visionSystem.UpdateVisible(visible);
			}
			Profiler.EndSample();

			_visionSystem.UpdateTextures();
		}
		
	}
}
