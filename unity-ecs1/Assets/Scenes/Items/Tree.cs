using UnityEngine;

namespace VirtualVillagers
{
	public class Tree : MonoBehaviour
	{
		[UnityEngine.SerializeField]
		protected Transform _modelTransform;

		public void SetSize(int size)
		{
			Vector3[] sizes =
			{
				new Vector3(0.25f, 0.25f, 0.25f),
				new Vector3(0.65f, 0.65f, 0.65f),
				new Vector3(1.0f, 1.0f, 1.0f),
			};

			if (size < 0)
				size = 0;
			if (size >= sizes.Length)
				size = sizes.Length - 1;
			
			_modelTransform.localScale = sizes[size];
		}
		
	}

}
