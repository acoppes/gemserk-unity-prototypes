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

	public DebugPanelButton AddButton(string text, Action<DebugPanelButton> callbackAction, Action<DebugPanelButton> updateAction)
	{
		var buttonObject = Instantiate(_buttonTemplate, _parent, false);
		buttonObject.SetActive(true);
		
		var buttonText = buttonObject.GetComponentInChildren<Text>();
		buttonText.text = text;
		
		var debugButton = buttonObject.GetComponent<DebugPanelButton>();
		debugButton.SetCallbackAction(callbackAction);
		debugButton.SetUpdateAction(updateAction);

		return debugButton;
	}
	
	public DebugPanelButton AddButton(string text, DebugPanelButton.Actions actions)
	{
		var buttonObject = Instantiate(_buttonTemplate, _parent, false);
		buttonObject.SetActive(true);
		
		var buttonText = buttonObject.GetComponentInChildren<Text>();
		buttonText.text = text;
		
		var debugButton = buttonObject.GetComponent<DebugPanelButton>();
		
		debugButton.SetCallbackAction(actions.callbackAction);
		debugButton.SetUpdateAction(actions.updateAction);
		debugButton.SetRefreshAction(actions.refreshAction);

		return debugButton;
	}

	public DebugPanelLabel AddLabel(string name, Action<DebugPanelLabel> updateAction)
	{
		var labelObject = Instantiate(_labelTemplate, _labelsParent, false);
		labelObject.SetActive(true);

		var debugLabel = labelObject.GetComponent<DebugPanelLabel>();
		debugLabel.SetUpdateAction(updateAction);

		return debugLabel;
	}
}