using System;
using Unity.Entities;
using UnityEngine;

public class Model : MonoBehaviour
{
	[NonSerialized]
	public Entity entity;

	public void UpdateRender(Transform transform)
	{
		// react to entity updated
		this.transform.SetPositionAndRotation(transform.position, transform.rotation);
	}
}
