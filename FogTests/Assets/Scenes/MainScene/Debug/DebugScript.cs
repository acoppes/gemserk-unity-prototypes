using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
	[SerializeField]
	private VisionSystem _visionSystem;

	[SerializeField]
	private CanvasGroup _fpsGroup;

	[SerializeField]
	protected Camera _mainCamera;

	[SerializeField]
	protected string _fogLayerName = "Fog";
	
	[SerializeField]
	protected string _visionLayerName = "Vision";

	[SerializeField]
	protected CanvasGroup _menuGroup;

	[SerializeField]
	protected PostProcessingBehaviour _postProcessing;

	[SerializeField]
	protected Text _moveCameraText;
	
	[SerializeField]
	protected GameObject _moveVision;

	[SerializeField]
	protected GameObject _moveCamera;

	[SerializeField]
	protected DebugPanelScript _debugPanelScript;
	
	private void Start()
	{
		ToggleMenuGroup();
		ToggleFpsText();
		_moveCameraText.text = _moveCamera.activeSelf ? "move vision" : "move camera";
	}
	
	public void ToggleMenuGroup()
	{
		_menuGroup.gameObject.SetActive(!_menuGroup.gameObject.activeSelf);
	}
	
	public void ToggleFogTexture()
	{
		var layerMask = LayerMask.GetMask(_fogLayerName);

		if ((_mainCamera.cullingMask & layerMask) == layerMask)
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask & ~layerMask;
		}
		else
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask | layerMask;
		}
	}
	
	public void ToggleVisionTexture()
	{
		var layerMask = LayerMask.GetMask(_visionLayerName );

		if ((_mainCamera.cullingMask & layerMask) == layerMask)
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask & ~layerMask;
		}
		else
		{
			_mainCamera.cullingMask = _mainCamera.cullingMask | layerMask;
		}
	}

	public void SwitchPlayerVision()
	{
		_visionSystem._activePlayers = (_visionSystem._activePlayers + 1) % _visionSystem.totalPlayers;
	}

	public void ToggleFpsText()
	{
		_fpsGroup.gameObject.SetActive(!_fpsGroup.gameObject.activeSelf);
	}

	public void ToggleRaycast()
	{
		_visionSystem.raycastEnabled = !_visionSystem.raycastEnabled;
	}

	public void ToggleBlur()
	{
		if (_postProcessing != null)
		{
			_postProcessing.enabled = !_postProcessing.enabled;
		}	
	}

	public void ToggleMoveCamera()
	{
		if (_moveCamera.activeSelf)
		{
			_moveVision.SetActive(true);
			_moveCamera.SetActive(false);
			_moveCameraText.text = "move camera";
		}
		else
		{
			_moveVision.SetActive(false);
			_moveCamera.SetActive(true);
			_moveCameraText.text = "move vision";
		}
	}
}
