using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(InterfaceAttribute))]
public class InterfaceTypePropertyDrawer : PropertyDrawer {

	const float propertyHeight = 17;

//	ParameterTargetType parameterTargetType;

//	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
//	{
//		var parameterTypeProperty = property.FindPropertyRelative ("parameterType");
//
//		Parameter.ParameterType type = (Parameter.ParameterType)parameterTypeProperty.intValue;
//
//		if (type == Parameter.ParameterType.None) {
//			return base.GetPropertyHeight (property, label);
//		} else if (type == Parameter.ParameterType.Value) {
//			return propertyHeight * 2;
//		}
//
//		return base.GetPropertyHeight (property, label);
//	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		var interfaceAttribute = attribute as InterfaceAttribute;

		var targetProperty = property.FindPropertyRelative ("target");

		var obj = targetProperty.objectReferenceValue;

		var allObjectsOfType = GameObject.FindObjectsOfType (interfaceAttribute.InterfaceType);

		var objectsList = allObjectsOfType.ToList ();

		int selectedObject = EditorGUI.Popup (position, interfaceAttribute.InterfaceType.Name, 
			objectsList.IndexOf(obj), allObjectsOfType.Select(t => t.name).ToArray());

		targetProperty.objectReferenceValue = objectsList [selectedObject];


//		obj = EditorGUI.ObjectField (position, obj, interfaceAttribute.InterfaceType, true);

//		targetProperty.objectReferenceValue = obj;
	}
		
}