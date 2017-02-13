using UnityEngine;

public class Planet : MonoBehaviour {

	public Sprite[] planets;

	public Transform model;

	public float minSize;
	public float maxSize;

	public void Randomize()
	{
		float scale = UnityEngine.Random.Range (minSize, maxSize);

		model.localScale = new Vector3 (scale, scale, 1.0f);

		var sprite = planets[UnityEngine.Random.Range(0, planets.Length)];

		var renderer = model.GetComponent<SpriteRenderer> ();
		renderer.sprite = sprite;

		float angle = UnityEngine.Random.Range (0, 360.0f);

		model.transform.localEulerAngles = new Vector3 (0, 0, angle);

		renderer.flipX = UnityEngine.Random.Range (-1.0f, 1.0f) > 0.0f;
		renderer.flipY = UnityEngine.Random.Range (-1.0f, 1.0f) > 0.0f;
	}
}
