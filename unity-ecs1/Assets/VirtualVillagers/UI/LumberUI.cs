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
		
		var lumber = lumberMill.GetComponent<LumberMill>();
		text.text = string.Format("Lumber: {0}", lumber.currentLumber);
	}
	
	// TODO: separate lumber concept between tree and lumber mill 
	// maybe call them lumber provider and consumer
}
