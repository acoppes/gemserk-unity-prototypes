using UnityEngine;

public class ShipPickup : MonoBehaviour {

	public int pickups;

	void OnTriggerEnter2D(Collider2D c) {
		var pickup = c.gameObject.GetComponent<Pickup> ();
		if (pickup != null) {
			pickups++;
			pickup.Take ();
		}
	}

}
