using UnityEngine;
using UnityEngine.UI;

public class DebugPanelLabel : MonoBehaviour
{
	[SerializeField]
	protected Text _text;

	public void UpdateText(string text)
	{
		_text.text = text;
	}
}