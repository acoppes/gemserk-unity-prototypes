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
			_text.text = _btComponent._behaviourTreeName;
	}
}
