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
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.75f, 0.75f, 0.75f),
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
