using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public void DrawPath(Vector3[] path)
    {
        lineRenderer.positionCount = path.Length;
        lineRenderer.SetPositions(path);
    }
}
