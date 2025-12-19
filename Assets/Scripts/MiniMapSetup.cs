using UnityEngine;

public class MiniMapSetup : MonoBehaviour
{
    public Camera miniMapCamera;  // Reference to your MiniMap Camera
    public RenderTexture miniMapTexture;  // Reference to the Render Texture

    void Start()
    {
        if (miniMapCamera != null && miniMapTexture != null)
        {
            miniMapCamera.targetTexture = miniMapTexture; // Assign the Render Texture to the Camera
        }
        else
        {
            Debug.LogError("MiniMap Camera or Render Texture is not assigned!");
        }
    }
}
