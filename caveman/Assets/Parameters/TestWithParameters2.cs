using UnityEngine;

public class TestWithParameters2 : MonoBehaviour {

	[ParameterTargetType(typeof(Capturable))]
	public Parameter capturable;
}
