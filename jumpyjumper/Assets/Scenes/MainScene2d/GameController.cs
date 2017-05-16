using UnityEngine;

public class GameController : MonoBehaviour {

	public Jumper jumper;

	public Score score;

	void Update()
	{
		score.SetScore (jumper.transform.position.y);		
	}

}
