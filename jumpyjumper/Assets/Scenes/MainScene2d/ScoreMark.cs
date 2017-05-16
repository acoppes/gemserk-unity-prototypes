using UnityEngine;

public class ScoreMark : MonoBehaviour {

	public Score score;

	public Jumper jumper;

	public Gradient barColor;

	public SpriteRenderer bar;

	public float maxDistance = 1.0f;

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (0, score.GetScore (), 0);

		float distance = transform.position.y - jumper.transform.position.y;

		bar.color = barColor.Evaluate (distance / maxDistance);

	}
}
