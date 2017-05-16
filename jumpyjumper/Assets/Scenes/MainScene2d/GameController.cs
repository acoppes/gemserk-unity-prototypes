using UnityEngine;

public class GameController : MonoBehaviour {

	public Jumper jumper;

	public Score score;

	void Update()
	{
		score.SetScore ((int) jumper.transform.position.y);		
	}

}
