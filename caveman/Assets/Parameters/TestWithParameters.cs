using UnityEngine;

public class TestWithParameters : MonoBehaviour {

	[ParameterTargetType(typeof(Switchable), "switchable")]
	public Parameter switchableParameter;

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
