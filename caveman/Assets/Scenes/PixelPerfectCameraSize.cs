using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectCameraSize : MonoBehaviour {

	public Camera myCamera;
	public int sizeReference = 4;

	void Update()
	{
		if (myCamera == null)
			return;

		myCamera.orthographicSize = myCamera.pixelHeight / sizeReference;
	}
}
