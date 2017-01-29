using UnityEngine;

public class TestWithParameters : MonoBehaviour {

	[ParameterTargetType(typeof(Switchable))]
	public Parameter switchable;

	void Start() 
	{
		var switchableValue = switchable.Get<Switchable>();

		if (switchableValue == null) {
			Debug.Log ("Not set");
		} else {
			Debug.Log ("Set as " + switchableValue.name);
		}
		
	}

}
