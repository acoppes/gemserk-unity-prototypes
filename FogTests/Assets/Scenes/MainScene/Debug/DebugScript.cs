using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
	[SerializeField]
	private VisionSystem _visionSystem;

	[SerializeField]
	private Text _fpsText;

	public void SwitchPlayerVision()
	{
		_visionSystem.currentPlayer = (_visionSystem.currentPlayer + 1) % _visionSystem.totalPlayers;
	}

	public void ToggleFpsText()
	{
		if (_fpsText != null)
		{
			_fpsText.gameObject.SetActive(!_fpsText.gameObject.active);
		}
	}
}
