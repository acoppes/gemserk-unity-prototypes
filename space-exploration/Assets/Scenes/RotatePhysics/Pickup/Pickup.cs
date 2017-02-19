using UnityEngine;

public class Pickup : MonoBehaviour, Randomizable {

//	public Collider pickupCollider;

	public void Randomize()
	{
		
	}

	public void Take()
	{
//		pickupCollider.enabled = false;
		// animation taken...

		GameObject.Destroy (this.gameObject);
	}

}
