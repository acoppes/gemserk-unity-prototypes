using UnityEngine;
using System;

public class ParameterTargetType : PropertyAttribute {

	readonly Type targetType;

	readonly string name = null;

	public Type GetTargetType()
	{
		return targetType;
	}

	public string Name {
		get { 
			return name;
		}
	}

	public ParameterTargetType (Type type) {
		this.targetType = type;
		this.name = null;
	}

	public ParameterTargetType (Type type, string name) {
		this.targetType = type;
		this.name = name;
	}
}
