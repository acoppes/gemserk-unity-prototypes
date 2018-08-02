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
	
	void Start()
	{
		_bounds = _worldBounds.bounds;
		_bounds.extents = _bounds.extents - 
		                  new Vector3(Screen.width, Screen.height, 0);	
	}
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var v3 = Input.mousePosition;
			v3 = _camera.ScreenToWorldPoint(v3);
			transform.position = v3;

			_pressedPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			var currentPosition = Input.mousePosition;
			var difference = currentPosition - _pressedPosition;
			
			_camera.transform.position = _camera.transform.position + difference * _speed * Time.deltaTime;

			_pressedPosition = currentPosition;
		}

		var p = _bounds.ClosestPoint(_camera.transform.position);
		p.z = _camera.transform.position.z;
		_camera.transform.position = p;
	}
	
}
