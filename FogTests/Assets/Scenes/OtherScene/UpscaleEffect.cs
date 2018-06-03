using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class UpscaleEffect : MonoBehaviour
{
    public Texture2D fogTexture;

    private Material material;

	public Shader shader;

    // Creates a private material used to the effect
    void Awake()
    {
        if (shader != null)
            material = new Material(shader);
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fogTexture == null || material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // material.SetTexture("_FogTex", fogTexture);
        Graphics.Blit(source, destination, material);

        // Debug.Log("Rendering");
    }
}
