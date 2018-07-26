using UnityEngine;

public class DebugScript : MonoBehaviour
{
	[SerializeField]
	private VisionSystem _visionSystem;

	public void SwitchPlayerVision()
	{
		_visionSystem.currentPlayer = (_visionSystem.currentPlayer + 1) % _visionSystem.totalPlayers;
	}	
}
