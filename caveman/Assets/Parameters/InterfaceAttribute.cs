using UnityEngine;
using System;

public class InterfaceAttribute : PropertyAttribute
{
	public Type InterfaceType {
		get;
		set;
	}

	public InterfaceAttribute(Type t)
	{
		InterfaceType = t;
	}
}
