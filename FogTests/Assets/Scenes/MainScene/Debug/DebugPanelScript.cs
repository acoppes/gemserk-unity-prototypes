using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugPanelScript : MonoBehaviour
{
	[SerializeField]
	protected Transform _parent;
	
	[SerializeField]
	protected Transform _labelsParent;

	[SerializeField]
	protected GameObject _buttonTemplate;

	[SerializeField]
	protected GameObject _labelTemplate;

	public void AddButton(string text, UnityAction action)
	{
		var buttonObject = Instantiate(_buttonTemplate, _parent, false);
		buttonObject.SetActive(true);

		var button = buttonObject.GetComponentInChildren<Button>();
		button.onClick.AddListener(action);
		
		var buttonText = buttonObject.GetComponentInChildren<Text>();
		buttonText.text = text;
	}

	public void AddLabel(string name, Action<DebugPanelLabel> updateAction)
	{
		var labelObject = Instantiate(_labelTemplate, _labelsParent, false);
		labelObject.SetActive(true);

		var debugLabel = labelObject.GetComponent<DebugPanelLabel>();

		StartCoroutine(UpdateLabel(debugLabel, updateAction));
	}

	private static IEnumerator UpdateLabel(DebugPanelLabel label, Action<DebugPanelLabel> updateAction)
	{
		while (true)
		{
			yield return null;
			updateAction(label);
		}
	}
}