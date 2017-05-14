using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;
using System;

public class ReflectionFieldUI : MonoBehaviour {

	public Text textName;
	public InputField inputFieldValue;
	public Toggle toggleValue;

	public void ConfigureForField(object o, FieldInfo field)
	{
		textName.text = field.Name;

		toggleValue.gameObject.SetActive (false);
		inputFieldValue.gameObject.SetActive (false);

		if (field.FieldType == typeof(int)) {
			ConfigureForInt (o, field);
		} else if (field.FieldType == typeof(string)) {
			ConfigureForString (o, field);
		} else if (field.FieldType == typeof(bool)) {
			ConfigureForBool (o, field);
		} else if (field.FieldType == typeof(float)) {
			ConfigureForFloat (o, field);
		}
	}

	void ConfigureForInt(object o, FieldInfo field)
	{
		inputFieldValue.gameObject.SetActive (true);

		inputFieldValue.contentType = InputField.ContentType.IntegerNumber;
		inputFieldValue.text = ((int) field.GetValue (o)).ToString();

		inputFieldValue.onValueChanged.AddListener (delegate(string newValue) {

			try { 
				var intvalue = int.Parse(newValue);
				field.SetValue(o, intvalue);
			} catch {
				// 
			}

		});
	}

	void ConfigureForFloat(object o, FieldInfo field)
	{
		inputFieldValue.gameObject.SetActive (true);

		inputFieldValue.contentType = InputField.ContentType.DecimalNumber;
		inputFieldValue.text = ((float) field.GetValue (o)).ToString();

		inputFieldValue.onValueChanged.AddListener (delegate(string newValue) {

			try { 
				var floatValue = float.Parse(newValue);
				field.SetValue(o, floatValue);
			} catch {
				// 
			}
				
		});
	}

	void ConfigureForString(object o, FieldInfo field)
	{
		inputFieldValue.gameObject.SetActive (true);

		inputFieldValue.contentType = InputField.ContentType.Standard;
		inputFieldValue.text = field.GetValue (o) as string;

		inputFieldValue.onValueChanged.AddListener (delegate(string newValue) {
			field.SetValue(o, newValue);
		});
	}

	void ConfigureForBool(object o, FieldInfo field)
	{
		toggleValue.gameObject.SetActive (true);

		toggleValue.isOn = (bool)field.GetValue (o);

		toggleValue.onValueChanged.AddListener (delegate(bool newValue) {
			field.SetValue(o, newValue);
		});
	}
}
