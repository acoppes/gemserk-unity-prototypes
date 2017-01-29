using UnityEngine;

[System.Serializable]
public class Parameter {

	public enum ParameterType
	{
		None = 0,
		Value = 1,
//		Variable = 2,
//		Query = 3
	}

	public ParameterType parameterType = ParameterType.Value;

	[SerializeField]
	Object targetReference;

	public T Get<T>() where T : Object
 	{
		// depending the type and stored data for each type....

		if (parameterType == ParameterType.Value) {
			return targetReference as T;
		}

		return null;
	}

}
