using UnityEngine;
using System;

public class ParameterTargetType : PropertyAttribute {

	readonly Type targetType;

	public Type GetTargetType()
	{
		return targetType;
	}

	public ParameterTargetType (Type type) {
		this.targetType = type;
	}
}
