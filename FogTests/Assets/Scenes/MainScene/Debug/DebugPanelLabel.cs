using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelLabel : MonoBehaviour
{
	[SerializeField]
	protected Text _text;

	private Action<DebugPanelLabel> _updateAction;
	
	public void UpdateText(string text)
	{
		_text.text = text;
	}

	public void SetUpdateAction(Action<DebugPanelLabel> updateAction)
	{
		_updateAction = updateAction;
	}

	private void Update()
	{
		_updateAction?.Invoke(this);
	}
}