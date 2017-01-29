using UnityEngine;

public class TestWithParameters2 : MonoBehaviour {

	[ParameterTargetType(typeof(Capturable))]
	public Parameter capturableParameter;

	[ParameterTargetType(typeof(Switchable))]
	public Parameter switchableParameter;
}
