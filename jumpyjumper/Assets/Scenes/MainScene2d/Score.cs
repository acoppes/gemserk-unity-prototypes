using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	public Text text;

	float _currentScore;

	public void SetScore(float score)
	{
		if (score < _currentScore)
			return;
		_currentScore = score;
		text.text = Mathf.RoundToInt(score).ToString ();
	}

	public float GetScore()
	{
		return _currentScore;
	}

}
