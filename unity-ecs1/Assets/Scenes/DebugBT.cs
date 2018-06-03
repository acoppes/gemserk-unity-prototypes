using FluentBehaviourTree;
using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class DebugBT : MonoBehaviour
{
	[SerializeField]
	protected Text _text;

	[SerializeField]
	protected BehaviourTreeComponent _btComponent;

	private void LateUpdate()
	{
		if (_text != null && _btComponent != null)
		{
			_text.text = string.Format("{0}.{1}", _btComponent._behaviourTreeName, _btComponent._debugCurrentAction);
		}
	}
}
