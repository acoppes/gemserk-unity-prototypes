using UnityEngine;

public class ScoreMark : MonoBehaviour {

	public Score score;

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (0, score.GetScore (), 0);
	}
}
