using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
//	public Vector3 targetPosition { get { return m_TargetPosition; } set { m_TargetPosition = value; } }
//	[SerializeField]
//	private Vector3 m_TargetPosition = new Vector3(1f, 0f, 2f);

	public bool showNames = true;
	public bool loop = true;
	
	public Vector3 nameOffset;
	public Vector3 newWaypointOffset;
	
	public List<Vector3> points = new List<Vector3>();
	
//	public virtual void Update()
//	{
//		transform.LookAt(m_TargetPosition);
//	}
}
