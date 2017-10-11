using UnityEngine;

public class HarvestableSpriteModel : HarvestableModel
{
	public SpriteRenderer spriteRenderer;

	public override void UpdateHarvestable (Harvestable harvestable)
	{
		var color = spriteRenderer.color;
		color.a = harvestable.current / harvestable.total;
		spriteRenderer.color = color;
	}
}