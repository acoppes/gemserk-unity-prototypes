using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {

	public float range = 100;

	public bool InRange(float x, float y) {
		return Vector2.Distance(new Vector2(x, y), transform.position) < range;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireSphere(transform.position, range);	
	}
	
}
