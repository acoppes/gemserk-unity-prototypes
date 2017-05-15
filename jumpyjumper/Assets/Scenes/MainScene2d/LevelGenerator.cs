using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public GameObject platformsParent;

	List<Platform> platforms = new List<Platform>();

	public float minVerticalDistance;
	public float maxVerticalDistance;

	public int platformsToGenerateCount;

	public float startVerticalPosition;

	public Bounds horizontalBounds;

	// Use this for initialization
	void Start () {

		platformsParent.GetComponentsInChildren<Platform> (false, platforms);

		Vector2 currentPosition = new Vector2 (0, startVerticalPosition);

		for (int i = 0; i < platformsToGenerateCount; i++) {
			currentPosition.x = UnityEngine.Random.Range (-horizontalBounds.size.x, horizontalBounds.size.x);

			var platformPrototype = platforms [UnityEngine.Random.Range (0, platforms.Count)];
			var platformInstance = GameObject.Instantiate (platformPrototype, this.transform);
			platformInstance.transform.localPosition = currentPosition;

			currentPosition.y += UnityEngine.Random.Range(minVerticalDistance, maxVerticalDistance);
//			currentHeight += UnityEngine.Random (minVerticalDistance, maxVerticalDistance);
		}	
	}

}
