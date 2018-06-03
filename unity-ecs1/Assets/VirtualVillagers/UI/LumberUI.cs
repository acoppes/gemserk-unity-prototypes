using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers.Components;

public class LumberUI : MonoBehaviour
{
	public GameObject lumberMill;

	public Text text;

	private void LateUpdate()
	{
		if (lumberMill == null)
			return;
		
		var btContext = lumberMill.GetComponent<BehaviourTreeContextComponent>();
		text.text = string.Format("Lumber: {0}", btContext.lumberMillLumberCurrent);
	}
}
