using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Model : MonoBehaviour
{
	[NonSerialized]
	public Entity entity;

    [SerializeField]
    private SpriteRenderer _sprite;

	public void UpdateRender(float2 lookingDirection, Transform transform)
	{
		// react to entity updated
		this.transform.SetPositionAndRotation(transform.position, transform.rotation);

        if (_sprite != null)
        {
            _sprite.flipX = lookingDirection.x < 0;
        }
    }
}
