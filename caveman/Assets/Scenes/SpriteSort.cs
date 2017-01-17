using UnityEngine;
using System.Collections.Generic;

public class SpriteSort : MonoBehaviour
{
	readonly List<SpriteRenderer> sprites = new List<SpriteRenderer>();

	void Awake()
	{
		GetComponentsInChildren<SpriteRenderer>(sprites);
	}

	void LateUpdate()
	{
		for (int i = 0; i < sprites.Count; i++) {
			var sprite = sprites [i];
			sprite.sortingOrder = Mathf.RoundToInt (transform.position.y * 10f) * -1;
		}
	}
}
