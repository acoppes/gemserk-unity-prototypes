using UnityEngine;

public class TestWithParameters : MonoBehaviour {

	public string pipote;

	[ParameterTargetType(typeof(Switchable), "Switchable")]
	public Parameter switchableParameter;

	public float superCount;

	void Start() 
	{
		var switchable = switchableParameter.Get<Switchable>();

		if (switchable == null) {
			Debug.Log ("Not set");
		} else {
			Debug.Log ("Set as " + switchable.name);
		}
		
	}

}
