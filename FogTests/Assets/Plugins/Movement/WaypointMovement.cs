using System;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
	[SerializeField]
	protected Waypoint _waypoint;

	[SerializeField]
	protected float _speed;
	
	private int _current;

	public void SetWaypoint(Waypoint waypoint)
	{
		_waypoint = waypoint;
		Start();
	}

	private void Start()
	{
		var best = 9999999.9f;
		if (_waypoint != null && _waypoint.points.Count > 0)
		{
			// get nearest waypoint to start...
			for (var i = 0; i < _waypoint.points.Count; i++)
			{
				var point = _waypoint.points[i];
				var distance = (point - transform.position).sqrMagnitude;
				if (distance < best)
				{
					_current = i;
					best = distance;
				}
			}
		}
	}

	private void FixedUpdate()
	{
		if (_waypoint == null || _waypoint.points.Count == 0)
			return;
		
		var current = Math.Min(_current, _waypoint.points.Count);
		var next = current >= _waypoint.points.Count - 1 ? 0 : current + 1;
		
		// ask for transformed point
		// var currentPoint = _waypoint.points[current];
		var nextPoint = _waypoint.points[next];

		var direction = (nextPoint - transform.position).normalized;

		var step = direction * _speed * Time.deltaTime;
		var nextPosition = transform.position + step;

		transform.position = nextPosition;
		
		if ((nextPosition - nextPoint).sqrMagnitude < step.sqrMagnitude)
		{
			_current = next;
		}
	}
}
