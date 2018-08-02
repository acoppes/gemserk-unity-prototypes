using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
	[SerializeField]
	private VisionSystem _visionSystem;

	[SerializeField]
	private Text _fpsText;

	[SerializeField]
	protected Camera _mainCamera;

	[SerializeField]
	protected string _fogLayerName = "Fog";
	
	public void ToggleFogTexture()
	{
		var fogMask = LayerMask.GetMask(_fogLayerName);

		if ((_mainCamera.cullingMask & fogMask) == fogMask)
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask & ~fogMask;
		}
		else
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask | fogMask;
		}
	}

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

	public void ToggleRaycast()
	{
		_visionSystem.raycastEnabled = !_visionSystem.raycastEnabled;
	}
}
