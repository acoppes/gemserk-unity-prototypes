using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visible : MonoBehaviour {

	private VisionSystem _visionSystem;

	private void Awake()
	{
		_visionSystem = FindObjectOfType<VisionSystem>();
	}
	
	// Update is called once per frame
	private void FixedUpdate()
	{
		// TODO: this should be done in the system too
		var visible = _visionSystem.IsVisible(transform.position);
		gameObject.layer = visible ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Hidden");
	}
}
