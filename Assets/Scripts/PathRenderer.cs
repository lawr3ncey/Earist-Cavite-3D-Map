using UnityEngine;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using System.Collections;

public class PathRenderer : MonoBehaviour
{
    public static PathRenderer Instance { get; private set; }

    public Transform playerTransform; // Reference to your player's transform
    public Vector3 targetPosition; // Set this via voice input
    public LineRenderer lineRenderer;
    public Vector3 fromPosition;
    public Vector3 toPosition;
    public GameObject firstPersonController; // Add this to reference the player
    public TextMeshProUGUI pathInfoText; // Drag and drop the UI Text here in the Inspector
    public VoiceAssistantOutput voiceAssistant;
    private bool isPlayerPathActive = false;
    private bool isEmergencyMode = false;
    private Transform nearestEmergencyExit;
    private Coroutine emergencyUpdateRoutine;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Set the thickness of the line
        lineRenderer.startWidth = 2.1f; // Adjust to make the line thicker at the start
        lineRenderer.endWidth = 2.1f;   // Adjust to make the line thicker at the end

        // Optional: Width curve for a variable thickness along the line
        lineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(2, 2.1f),     // Start width (e.g., thicker)
            new Keyframe(5.1f, 5.1f),  // Middle width (e.g., thicker still)
            new Keyframe(2, 2.1f)      // End width (same as start)
        );
    }


    public void SetEmergencyMode(bool isActive)
    {
        isEmergencyMode = isActive;

        if (isEmergencyMode)
        {
            nearestEmergencyExit = FindNearestEmergencyExit(playerTransform.position);

            if (nearestEmergencyExit != null)
            {
                DrawPath(playerTransform.position, nearestEmergencyExit.position, true);
                Debug.Log($"Emergency Path Found: Follow the glowing line to {nearestEmergencyExit.name}.");
                // Start updating emergency path dynamically
                if (emergencyUpdateRoutine == null)
                {
                    emergencyUpdateRoutine = StartCoroutine(UpdateEmergencyPath());
                }

            }
            else
            {
                Debug.Log("No emergency exits found.");
            }
        }
        else
        {
            // Stop updating when exiting emergency mode
            if (emergencyUpdateRoutine != null)
            {
                StopCoroutine(emergencyUpdateRoutine);
                emergencyUpdateRoutine = null;
            }

            // Clear the emergency path only if it was active
            if (lineRenderer.positionCount > 0)
            {
                lineRenderer.positionCount = 0;
            }

            Debug.Log("Emergency Mode Deactivated: Path cleared.");
        }
    }


    public void DrawPath(Vector3 start, Vector3 end, bool isPlayerPath = false)
    {
        fromPosition = isPlayerPath ? playerTransform.position : start;
        toPosition = end;
        isPlayerPathActive = isPlayerPath;

        if (fromPosition == Vector3.zero || toPosition == Vector3.zero)
        {
            Debug.LogError("Locations are not set!");
            return;
        }

        // Find the GameObjects at the "from" and "to" positions using their tags
        GameObject fromObject = FindClosestObjectWithTag("Location", fromPosition);
        GameObject toObject = FindClosestObjectWithTag("Location", toPosition);

        string fromName = fromObject != null ? fromObject.name : "Unknown";
        string toName = toObject != null ? toObject.name : "Unknown";

        // Debug log with location names
        Debug.Log($"Path connected from {fromName} to {toName}. Please look for the white glowing line.");

        // Trigger voice output to speak the result
        if (voiceAssistant != null)
        {
            voiceAssistant.Speak($"Path connected from {fromName} to {toName}. Please look for the white glowing line.");
        }
        else
        {
            Debug.LogError("VoiceAssistantOutput is not found in the scene.");
        }


        ABPath path = ABPath.Construct(fromPosition, toPosition, OnPathComplete);
        AstarPath.StartPath(path);
    }

    private GameObject FindClosestObjectWithTag(string tag, Vector3 position)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    private Transform FindNearestEmergencyExit(Vector3 playerPos)
    {
        Transform nearestExit = null;
        float minDistance = float.MaxValue;

        foreach (GameObject exit in GameObject.FindGameObjectsWithTag("Emergency"))
        {
            float distance = Vector3.Distance(playerPos, exit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestExit = exit.transform;
            }
        }

        return nearestExit;
    }

    private IEnumerator UpdateEmergencyPath()
    {
        Vector3 lastPosition = playerTransform.position;

        while (isEmergencyMode)
        {
            // Find the nearest emergency exit dynamically
            Transform newNearestExit = FindNearestEmergencyExit(playerTransform.position);

            if (newNearestExit != null && newNearestExit != nearestEmergencyExit)
            {
                nearestEmergencyExit = newNearestExit;
                DrawPath(playerTransform.position, nearestEmergencyExit.position, true);
                Debug.Log($"Emergency Path Updated: New nearest exit is {nearestEmergencyExit.name}.");
            }

            // Update the path if the player moves more than 2 meters
            if (Vector3.Distance(playerTransform.position, lastPosition) > 2.0f)
            {
                DrawPath(playerTransform.position, nearestEmergencyExit.position, true);
                lastPosition = playerTransform.position;
                Debug.Log("Emergency Path Updated: Player moved.");
            }

            yield return new WaitForSeconds(1.0f); // Adjust update frequency as needed
        }
    }


    private void Update()
    {
        if (lineRenderer.positionCount > 0 && isPlayerPathActive)
        {
            UpdatePath();
        }
    }

    private void UpdatePath()
    {
        // Recalculate the path from the first-person controller's current position to the target position
        fromPosition = firstPersonController.transform.position;

        // Create and start a new path calculation
        ABPath path = ABPath.Construct(fromPosition, toPosition, OnPathComplete);
        AstarPath.StartPath(path);

    }

    void OnPathComplete(Path p)
    {
        if (p.error)
        {
            Debug.LogError("Path calculation failed.");
            return;
        }

        float computationTime = p.duration * 1000; // Convert to milliseconds if needed
        int searchedNodes = p.searchedNodes;

        float playerHeightMidpoint = 1.0f; // Adjust as needed for the player's height
        List<Vector3> waypoints = p.vectorPath;
        List<Vector3> adjustedWaypoints = new List<Vector3>();

        // Declare and initialize totalPathLength here
        float totalPathLength = 0.0f;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 adjustedWaypoint = waypoints[i] + new Vector3(0, playerHeightMidpoint, 0);
            adjustedWaypoints.Add(adjustedWaypoint);

            if (i > 0)
            {
                totalPathLength += Vector3.Distance(adjustedWaypoints[i - 1], adjustedWaypoint);
            }
        }

        // Convert totalPathLength to meters
        float totalPathLengthInMeters = totalPathLength / 4;

        // Set positions in line renderer
        lineRenderer.positionCount = adjustedWaypoints.Count;
        lineRenderer.SetPositions(adjustedWaypoints.ToArray());

        // Display path information in meters
        DisplayPathInfo(computationTime, searchedNodes, totalPathLengthInMeters);

        // Log debug information with matching path length
        Debug.Log($"Path Completed : Computation Time {computationTime:F2} ms, Searched Nodes {searchedNodes}, Converted Path Length to Meter {totalPathLengthInMeters:F2}meter");
    }


    // Call this function after computing the path to update path information on the screen
    void DisplayPathInfo(float computationTime, int searchedNodes, float totalPathLengthInMeters)
    {
        // Format the text to be on a single line
        pathInfoText.text = $"Computation Time: {computationTime:F2} ms, Searched Nodes: {searchedNodes}, Distance: {totalPathLengthInMeters:F2} meter/s";
    }

    public void ClearPath()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0; // Clears the path
        }

        isEmergencyMode = false; // Turn off emergency mode if active
        StopAllCoroutines(); // Stop updating the path

        if (voiceAssistant != null)
        {
            voiceAssistant.Speak("Path is cleared");
        }
        else
        {
            Debug.LogError("VoiceAssistant not found in the scene.");
        }

        Debug.Log("Path is cleared.");
    }




}