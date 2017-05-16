using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text text;

	int _currentScore;

	public void SetScore(int score)
	{
		if (score < _currentScore)
			return;
		_currentScore = score;
		text.text = score.ToString ();
	}
}
