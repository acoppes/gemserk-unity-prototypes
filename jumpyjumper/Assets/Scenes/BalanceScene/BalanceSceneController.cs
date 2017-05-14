using UnityEngine;

public class BalanceSceneController : MonoBehaviour {

	public GameObject uiPrefab;
	public GameObject parent;

	// Use this for initialization
	void Start () {
		new UIReflectionGenerator (uiPrefab).CreateUI (parent, new BalanceConfiguration ());	
	}

}
