using UnityEngine;
using System;

public class Harvestable : MonoBehaviour {

	public float total;
	public float current;

	public GameObject positionReference;

	public HarvestableModel harvestableModel;

	[NonSerialized]
	public World world;

	const float epsilon = 0.01f;

	HarvestSystem _harvestSystem;

	void Start()
	{
		_harvestSystem = world.GetComponent<HarvestSystem>();
		if (_harvestSystem != null)
			_harvestSystem.Register (this);
	}

	public bool IsDepleted ()
	{
		return current < epsilon;
	}

	public void Harvest(float harvest)
	{
		current -= harvest;
		if (harvestableModel != null)
			harvestableModel.UpdateHarvestable (this);
	}
}


