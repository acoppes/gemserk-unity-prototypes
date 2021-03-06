﻿using UnityEngine;
using UnityEditor;
using System.Globalization;
using System;

[CustomPropertyDrawer(typeof(Parameter))]
public class ParameterCustomPropertyDrawer : PropertyDrawer {

	const float propertyHeight = 17;

	ParameterTargetType parameterTargetType;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		var parameterTypeProperty = property.FindPropertyRelative ("parameterType");

		Parameter.ParameterType type = (Parameter.ParameterType)parameterTypeProperty.intValue;

		if (type == Parameter.ParameterType.None) {
			return base.GetPropertyHeight (property, label);
		} else if (type == Parameter.ParameterType.Value) {
			return propertyHeight * 2;
		}

		return base.GetPropertyHeight (property, label);
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		var typeNames = new string[] {
			Parameter.ParameterType.None.ToString (),
			Parameter.ParameterType.Value.ToString (),
//			Parameter.ParameterType.Variable.ToString (),
//			Parameter.ParameterType.Query.ToString ()
		};

		var typeRect = new Rect (new Vector2 (position.x, position.y), 
			               new Vector2 (position.size.x, propertyHeight));

		parameterTargetType = GetParameterType<ParameterTargetType>(property, attribute);

		var name = property.displayName;

		if (parameterTargetType != null && parameterTargetType.Name != null) {
			name = parameterTargetType.Name;
		}

		var parameterTypeProperty = property.FindPropertyRelative ("parameterType");
		parameterTypeProperty.intValue = EditorGUI.Popup (typeRect, name, parameterTypeProperty.intValue, typeNames);

		Parameter.ParameterType type = (Parameter.ParameterType)parameterTypeProperty.intValue;

		if (type == Parameter.ParameterType.None) {

			ResetSerializedValues (property);	

		} else if (type == Parameter.ParameterType.Value) {
			var referenceRect = new Rect (new Vector2 (position.x, position.y + propertyHeight), 
				new Vector2 (position.size.x, propertyHeight));
			
			DrawValue (referenceRect, property, label);	
		}
			
//		typeof(MonoBehaviour).get
	}

	void ResetSerializedValues (SerializedProperty property)
	{
		var referenceProperty = property.FindPropertyRelative ("targetReference");
		referenceProperty.objectReferenceValue = null;
	}

	void DrawValue(Rect position, SerializedProperty property, GUIContent label)
	{
		var referenceProperty = property.FindPropertyRelative ("targetReference");

		var referenceType = typeof(UnityEngine.Object);
//		var name = property.displayName;

//		var parameterTargetType = GetParameterType(property);

		if (parameterTargetType != null) {
			referenceType = parameterTargetType.GetTargetType ();
		
//			if (parameterTargetType.Name != null) {
//				name = parameterTargetType.Name;
//			}
		}
			
		referenceProperty.objectReferenceValue = EditorGUI.ObjectField (position, GUIContent.none, 
			referenceProperty.objectReferenceValue, referenceType, true);
		
		var newValue = referenceProperty.objectReferenceValue;

		if (newValue != null && !referenceType.IsAssignableFrom (newValue.GetType ())) {
			// if previously serialized object was not of the parameter type, then unset serialised value
			referenceProperty.objectReferenceValue = null;
		}
	}

	// workaround to get attribute of a property when not using the PropertyDrawer using PropertyAttribute
	static T GetParameterType<T>(SerializedProperty property, PropertyAttribute attribute) where T : Attribute
	{
		var parameterTargetType = attribute as T;

		if (parameterTargetType != null)
			return parameterTargetType;

		var targetObject = property.serializedObject.targetObject;

		if (targetObject == null)
			return null;

		var targetObjectType = targetObject.GetType ();

		var attributes = targetObjectType.GetField (property.name)
			.GetCustomAttributes(typeof(ParameterTargetType), true);

		if (attributes == null || attributes.Length == 0)
			return null;

		return attributes [0] as T;
	}

//	string Capitalize(string str)
//	{
//		return CultureInfo.CurrentCulture.TextInfo.ToTitleCase (str);
//	}


}
