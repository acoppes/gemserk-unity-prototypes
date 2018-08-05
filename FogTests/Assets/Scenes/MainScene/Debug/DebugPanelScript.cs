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

	public void AddButton(string text, Action<DebugPanelButton> callbackAction, Action<DebugPanelButton> updateAction)
	{
		var buttonObject = Instantiate(_buttonTemplate, _parent, false);
		buttonObject.SetActive(true);
		
		var buttonText = buttonObject.GetComponentInChildren<Text>();
		buttonText.text = text;
		
		var debugLabel = buttonObject.GetComponent<DebugPanelButton>();
		debugLabel.SetCallbackAction(callbackAction);
		debugLabel.SetUpdateAction(updateAction);
	}

	public void AddLabel(string name, Action<DebugPanelLabel> updateAction)
	{
		var labelObject = Instantiate(_labelTemplate, _labelsParent, false);
		labelObject.SetActive(true);

		var debugLabel = labelObject.GetComponent<DebugPanelLabel>();
		debugLabel.SetUpdateAction(updateAction);
	}
}