using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugPanelScript : MonoBehaviour
{
	[SerializeField]
	protected Transform _parent;

	[SerializeField]
	protected GameObject _buttonTemplate;
	
	public void AddButton(string text, UnityAction action)
	{
		var buttonObject = Instantiate(_buttonTemplate, _parent, false);
		buttonObject.SetActive(true);

		var button = buttonObject.GetComponentInChildren<Button>();
		button.onClick.AddListener(action);
		
		var buttonText = buttonObject.GetComponentInChildren<Text>();
		buttonText.text = text;
	}
}