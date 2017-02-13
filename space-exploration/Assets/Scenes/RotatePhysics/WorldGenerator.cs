using UnityEngine;

public class WorldGenerator : MonoBehaviour {

	public float width;
	public float height;

	public float planetWidth;
	public float planetHeight;

	public Planet planetPrefab;

	public float distribution = 0.25f;

	void Awake()
	{
		GenerateWorld ();
	}

	public void GenerateWorld()
	{
		float planetSize = planetWidth * planetHeight;

		int total = Mathf.RoundToInt(((width * height) / (planetSize)) * distribution);

		for (int i = 0; i < total; i++) {
			float x = UnityEngine.Random.Range (-width / 2, width / 2);
			float y = UnityEngine.Random.Range (-height / 2, height / 2);

			var planet = GameObject.Instantiate (planetPrefab) as Planet;
			planet.transform.SetParent (this.transform);

			planet.transform.position = new Vector3 (x, y, 0);
			planet.Randomize();
		}
	}

}
