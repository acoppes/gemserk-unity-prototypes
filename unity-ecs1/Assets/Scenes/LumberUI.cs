using UnityEngine;
using UnityEngine.UI;
using VirtualVillagers;

public class LumberUI : MonoBehaviour
{
	public GameObject lumberMill;

	public Text text;

	private void LateUpdate()
	{
		var btContext = lumberMill.GetComponent<BehaviourTreeContextComponent>();
		text.text = string.Format("Lumber: {0}", btContext.lumberMillLumberCurrent);
	}
}
