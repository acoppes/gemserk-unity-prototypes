using UnityEngine;
using UnityEditor;

public static class RoundPositionsScript {

	[MenuItem("Extras/Round positions to pixel perfect")]
	public static void RoundPositions()
	{
		GameObject[] gameObjects = UnityEditor.Selection.gameObjects;
		foreach (var gameObject in gameObjects) {
			Vector3 position = gameObject.transform.position;
			gameObject.transform.position = new Vector3 (Mathf.RoundToInt (position.x), Mathf.RoundToInt (position.y), Mathf.RoundToInt (position.z));
		}
	}
}
