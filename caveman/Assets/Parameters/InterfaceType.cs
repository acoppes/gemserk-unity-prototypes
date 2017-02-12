using UnityEngine;
using System;

[Serializable]
public class InterfaceType {

	public MonoBehaviour target;

	public T Get<T> () where T : class
	{
		return target as T;
	}

}