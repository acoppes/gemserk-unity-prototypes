using UnityEngine;

namespace Gemserk.Vision
{
	public class VisionCamera : MonoBehaviour
	{
		[SerializeField]
		protected Camera _camera;

		[SerializeField]
		protected Transform _fogSprite;

		public Vector2 GetScale(int width, int height)
		{
			return new Vector2(_fogSprite.localScale.x / width, _fogSprite.localScale.y / height);
		}
	
	}
}
