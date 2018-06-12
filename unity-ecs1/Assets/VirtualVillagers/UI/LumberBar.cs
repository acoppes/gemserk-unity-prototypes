using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class LumberBar : MonoBehaviour
{
	[SerializeField]
	protected CanvasGroup _canvasGroup;
	
	[SerializeField]
	protected Image _current;

	[SerializeField]
	protected LumberUIComponent _lumber;
	
	[SerializeField]
	protected RectTransform _container;
	
	// Update is called once per frame
	private void LateUpdate()
	{
		if (_lumber == null)
			return;

		_canvasGroup.alpha = _lumber.visible ? 1.0f : 0.0f;
		
		_current.fillAmount = _lumber.current / _lumber.total;

		var size = _lumber.size == 0 ? 35.0f : 70.0f;
		_container.sizeDelta = new Vector2(size, _container.sizeDelta.y);
	}
}
