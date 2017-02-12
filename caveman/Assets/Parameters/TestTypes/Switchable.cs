using UnityEngine;
using System;

//[Serializable]
//public class CapturableInterfaceType : InterfaceType<ICapturable>
//{
//	
//}

public class ICapturableContainer : InterfaceType
{
	ICapturable capturable;

	public ICapturable Capturable {
		get {
			if (capturable == null)
				capturable = base.Get<ICapturable> ();
			return capturable;
		}
	}
}

public class Switchable : MonoBehaviour {

//	public InterfaceType capturable;


//	public MonoBehaviour capturable;

//	public CapturableInterfaceType capturable2;

//	[InterfaceAttribute(typeof(ICapturable))]
//	public InterfaceType capturable2;
//
//	[InterfaceAttribute(typeof(ICapturable))]
//	public ICapturableContainer capturable3;

	// Use this for initialization
	void Start () {
//		capturable2.Get<ICapturable> ().Capture ();
//		capturable.Get<ICapturable>().Capture ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
