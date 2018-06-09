using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class LumberBar : MonoBehaviour
{
	[SerializeField]
	protected Image _current;

	[SerializeField]
	protected LumberComponent _lumber;
	
	// Update is called once per frame
	void LateUpdate()
	{
		if (_lumber == null)
			return;

		_current.fillAmount = _lumber.current / _lumber.total;
	}
}
