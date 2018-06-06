using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class LumberUI : MonoBehaviour
{
	[SerializeField]
	protected Text text;

	[SerializeField]
	protected LumberMillUI _ui;

	private void LateUpdate()
	{
		if (_ui == null)
			return;
		text.text = string.Format("Lumber: {0}", _ui.currentLumber);
	}
}
