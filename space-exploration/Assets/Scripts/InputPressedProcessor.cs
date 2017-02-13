using UnityEngine;
using System;

[Serializable]
public class InputPressedProcessor
{
	public string button;

	bool value;

	public bool Value {
		get {
			return value;
		}
	}

	public void Update()
	{
		value = Input.GetButton (button);
	}

}

[Serializable]
public class InputAxisProcessor
{
	public string axis;

	float value;

	public bool raw = false;

	public float Value {
		get {
			return value;
		}
	}

	public void Update()
	{
		if (raw)
			value = Input.GetAxisRaw (axis);
		else
			value = Input.GetAxis (axis);
	}

}
