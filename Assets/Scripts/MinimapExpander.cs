using UnityEngine;
using UnityEngine.UI;

public class MinimapExpander : MonoBehaviour
{
    public RectTransform minimapRect;  // The RectTransform of the minimap
    public Vector2 expandedSize = new Vector2(500, 450);  // Expanded size
    public Vector2 defaultSize = new Vector2(250, 225);   // Default size
    private bool isExpanded = false;  // Track if minimap is expanded

    public void ToggleMinimapSize()
    {
        if (isExpanded)
        {
            // Shrink to default size
            minimapRect.sizeDelta = defaultSize;
        }
        else
        {
            // Expand to large size
            minimapRect.sizeDelta = expandedSize;
        }

        isExpanded = !isExpanded; // Toggle state
    }
}
