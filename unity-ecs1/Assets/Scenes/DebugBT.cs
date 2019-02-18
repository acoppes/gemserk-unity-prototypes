using FluentBehaviourTree;
using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class DebugBT : MonoBehaviour
{
	[SerializeField]
	protected Text _text;

	public BehaviourTreeComponent behaviourTreeComponent;

	private void LateUpdate()
	{
		if (_text != null && behaviourTreeComponent != null)
		{
			_text.text = string.Format("{0}.{1}", behaviourTreeComponent._behaviourTreeName, behaviourTreeComponent._debugCurrentAction);
		}
	}
}
