using UnityEngine;
using System;
using System.Reflection;

public class UIReflectionGenerator
{
	readonly GameObject _uiObjectPrefab;

	public UIReflectionGenerator(GameObject uiObjectPrefab)
	{
		this._uiObjectPrefab = uiObjectPrefab;
	}

	public void CreateUI(GameObject uiParent, System.Object o)
	{
		Type type = o.GetType ();

		FieldInfo[] fields = type.GetFields();

		foreach (var field in fields) {
			CreateStringElement (uiParent, o, field);
		}

	}

	void CreateStringElement(GameObject uiParent, object o, FieldInfo field)
	{
		GameObject gameObject = GameObject.Instantiate(_uiObjectPrefab);

		gameObject.SetActive (true);

		gameObject.transform.SetParent (uiParent.transform, false);
		ReflectionFieldUI reflectionField = gameObject.GetComponent<ReflectionFieldUI> ();

		reflectionField.ConfigureForField (o, field);
	}
}
