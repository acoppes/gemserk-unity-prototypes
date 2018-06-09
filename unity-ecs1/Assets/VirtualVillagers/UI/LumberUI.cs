using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class LumberUI : MonoBehaviour
{
	[SerializeField]
	protected Text text;

	[SerializeField]
	protected LumberMillUIComponent _uiComponent;

	private void LateUpdate()
	{
		if (_uiComponent == null)
			return;
		text.text = string.Format("Lumber: {0}", _uiComponent.currentLumber);
	}
}
