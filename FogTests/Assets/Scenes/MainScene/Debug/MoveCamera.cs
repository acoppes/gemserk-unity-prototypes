using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	[SerializeField]
	protected Camera _camera;

	[SerializeField]
	protected float _speed = 1.0f;

	[SerializeField]
	protected BoxCollider2D _worldBounds;
	
	private Vector3 _pressedPosition;

	private Bounds _bounds;

	[SerializeField]
	protected bool _truncatePosition = false;

	private void Start()
	{
		_bounds = _worldBounds.bounds;
		_bounds.extents = _bounds.extents - 
		                  new Vector3(Screen.width, Screen.height, 0);	
	}
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_pressedPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			var currentPosition = Input.mousePosition;

			var p0 = _camera.ScreenToWorldPoint(_pressedPosition);
			var p1 = _camera.ScreenToWorldPoint(currentPosition);
			
			var difference = p1 - p0;
			
			_camera.transform.position = _camera.transform.position + difference * _speed;

			_pressedPosition = currentPosition;
		}

		if (_truncatePosition)
		{
			//var sp = _camera.WorldToScreenPoint(_camera.transform.position);
			var sp = _camera.transform.position;
			var p = _bounds.ClosestPoint(sp);
			p.z = sp.z;
			_camera.transform.position = p;
		
			// _camera.transform.position = _camera.ScreenToWorldPoint(p);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position + _bounds.center, _bounds.size);
	}
}
