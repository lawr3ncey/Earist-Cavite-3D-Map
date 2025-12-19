using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject floorPrefab;  
    public GameObject stairsconnectPrefab;
    public GameObject roofPrefab;
    public GameObject stairsPrefab;
    public GameObject groundfloorPrefab;
    public GameObject wallPrefab;
    public GameObject balconyPrefab;
    public GameObject walldoorPrefab;
    
    private bool isDragging = false;
    private GameObject selectedPrefab;
    private GameObject previewObject;
    private bool isPlacingBuildingPart = false;
    public LayerMask environmentLayers;
    public float gridSize = 1f;

    private GameObject selectedBuildingPart; // Tracks the currently selected building part
    public float[] allowedRotationAngles = { 0f, 90f, 180f, 270f }; // Predefined snap angles
    private int currentRotationIndex = 0; // Default index for allowed angles

    public float moveStep = 1f; // Amount to move per button press
    private Stack<GameObject> placedBuildingParts = new Stack<GameObject>();

    public GameObject buttonGroup; // Reference to the ButtonGroup containing arrow, undo, and delete buttons
    public GameObject buildButton; // Reference to the BuildButton (always visible)
    private bool isBuildModeActive = false; // Tracks whether build mode is active

    void Start()
    {
        // Ensure buttonGroup is hidden when the game starts
        buttonGroup.SetActive(false);

        // Ensure the buildButton is always visible
        if (buildButton != null)
        {
            buildButton.SetActive(true);
        }
    }

    public void ToggleBuildMode()
    {
        isBuildModeActive = !isBuildModeActive; // Toggle the build mode state

        // Show or hide the button group
        buttonGroup.SetActive(isBuildModeActive);

        if (isBuildModeActive)
        {
            Debug.Log("Build mode activated. Buttons are visible.");
        }
        else
        {
            Debug.Log("Build mode deactivated. Buttons are hidden.");
        }
    }

    void Update()
    {
        #if UNITY_IOS || UNITY_ANDROID
        if (!isPlacingBuildingPart) // Only allow dragging when not in placement mode
        {
            HandleTouchInput();
        }
        else
        {
            HandlePlacement(); // Handle preview placement logic
        }
        #endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (touch.phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, environmentLayers))
                {
                    if (hit.collider.CompareTag("BuildingPart"))
                    {
                        // Select placed building part
                        selectedBuildingPart = hit.collider.gameObject;
                        Debug.Log("Selected: " + selectedBuildingPart.name);
                        isDragging = true; // Start dragging
                    }
                    else
                    {
                        selectedBuildingPart = null;
                        Debug.Log("No building part selected.");
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging && selectedBuildingPart != null)
            {
                // Update the selected object's position
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, environmentLayers))
                {
                    Vector3 snappedPosition = SnapToGrid(hit.point);
                    selectedBuildingPart.transform.position = snappedPosition;
                    Debug.Log("Dragging " + selectedBuildingPart.name + " to " + snappedPosition);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Stop dragging
                isDragging = false;
                Debug.Log("Stopped dragging.");
            }
        }
    }

    // New method for rotating the selected object
    public void RotateSelectedObject()
    {
        if (selectedBuildingPart != null)
        {
            // Cycle through the predefined angles
            currentRotationIndex = (currentRotationIndex + 1) % allowedRotationAngles.Length;

            // Apply the rotation to the selected building part
            float snappedAngle = allowedRotationAngles[currentRotationIndex];
            selectedBuildingPart.transform.rotation = Quaternion.Euler(0f, snappedAngle, 0f);

            Debug.Log("Rotated " + selectedBuildingPart.name + " to " + snappedAngle + "°");
        }
        else
        {
            Debug.Log("No building part selected to rotate.");
        }
    }

    public void SelectBuildingPart(string part)
    {
        switch (part)
        {
            case "Floor":
                selectedPrefab = floorPrefab;
                break;
            case "Staircase":
                selectedPrefab = stairsconnectPrefab;
                break;
            case "Roof":
                selectedPrefab = roofPrefab;
                break;
            case "Stairs":
                selectedPrefab = stairsPrefab;
                break;
            case "Ground":
                selectedPrefab = groundfloorPrefab;
                break;
            case "Wall":
                selectedPrefab = wallPrefab;
                break;
            case "Balcony":
                selectedPrefab = balconyPrefab;
                break;
            case "Door":
                selectedPrefab = walldoorPrefab;
                break;
        }

        if (selectedPrefab != null)
        {
            Debug.Log("Preview object instantiated.");
            StartPlacement();
        }
    }

    

    public void StartPlacement()
    {
        isPlacingBuildingPart = true;

        if (selectedPrefab != null)
        {
            if (previewObject != null) Destroy(previewObject); // Remove any existing preview object
            previewObject = Instantiate(selectedPrefab);
            previewObject.GetComponent<Collider>().enabled = false; // Disable collider for preview
            Debug.Log("Preview object instantiated.");
        }
        else
        {
            Debug.LogWarning("No prefab selected to instantiate a preview.");
        }   
    }

    public void StopPlacement()
    {
        isPlacingBuildingPart = false;

        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null; // Reset preview object reference
        }

        Debug.Log("Placement stopped.");
    }

    // Move the selected object Up
    public void MoveUp()
    {
        if (selectedBuildingPart != null)
        {
            Vector3 newPosition = selectedBuildingPart.transform.position + new Vector3(0f, moveStep, 0f);
            selectedBuildingPart.transform.position = SnapToGrid(newPosition);
            Debug.Log("Moved Up: " + selectedBuildingPart.name);
        }
    }

    // Move the selected object Down
    public void MoveDown()
    {
        if (selectedBuildingPart != null)
        {
            Vector3 newPosition = selectedBuildingPart.transform.position - new Vector3(0f, moveStep, 0f);
            selectedBuildingPart.transform.position = SnapToGrid(newPosition);
            Debug.Log("Moved Down: " + selectedBuildingPart.name);
        }
    }

    // Move the selected object Left
    public void MoveLeft()
    {
        if (selectedBuildingPart != null)
        {
            Vector3 newPosition = selectedBuildingPart.transform.position - new Vector3(moveStep, 0f, 0f);
            selectedBuildingPart.transform.position = SnapToGrid(newPosition);
            Debug.Log("Moved Left: " + selectedBuildingPart.name);
        }
    }

    // Move the selected object Right
    public void MoveRight()
    {
        if (selectedBuildingPart != null)
        {
            Vector3 newPosition = selectedBuildingPart.transform.position + new Vector3(moveStep, 0f, 0f);
            selectedBuildingPart.transform.position = SnapToGrid(newPosition);
            Debug.Log("Moved Right: " + selectedBuildingPart.name);
        }
    }

    public void RemoveBuildingPart()
    {
        if (selectedBuildingPart != null)
        {
            // Destroy the selected building part
            Destroy(selectedBuildingPart);
            Debug.Log("Building part removed.");

            // Optionally, reset selection after removing it
            selectedBuildingPart = null;
        }
        else
        {
            Debug.Log("No building part selected to remove.");
        }
    }

    public void UndoPlacement()
    {
        if (placedBuildingParts.Count > 0)
        {
            // Pop the last placed object from the stack and destroy it
            GameObject lastPlacedObject = placedBuildingParts.Pop();
            Destroy(lastPlacedObject);
            Debug.Log("Last placed building part removed.");
        }
        else
        {
            Debug.Log("No building part to undo.");
        }
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(x, y, z);
    }

    private void UpdatePreviewPosition(Vector3 position)
    {
        if (previewObject == null) return;

        // Update the position with snapping
        previewObject.transform.position = SnapToGrid(position);

        // Snap the rotation to the current direction
        float snappedAngle = allowedRotationAngles[currentRotationIndex];
        previewObject.transform.rotation = Quaternion.Euler(0f, snappedAngle, 0f);
    }


    private void HandlePlacement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, environmentLayers))
            {
                if (previewObject != null)
                {
                    UpdatePreviewPosition(hit.point); // Update preview position
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Place the object on touch release
                    float snappedAngle = allowedRotationAngles[currentRotationIndex];
                    Quaternion snappedRotation = Quaternion.Euler(0f, snappedAngle, 0f);
                    GameObject placedObject = Instantiate(selectedPrefab, SnapToGrid(hit.point), snappedRotation);

                    placedObject.tag = "BuildingPart"; // Ensure placed objects are tagged
                    placedObject.GetComponent<Collider>().enabled = true; // Re-enable collider for interaction

                    // Push the placed object onto the stack
                    placedBuildingParts.Push(placedObject);

                    Debug.Log("Placed " + selectedPrefab.name + " at " + SnapToGrid(hit.point) + " with rotation " + snappedAngle);

                    // Stop placement and destroy preview
                    StopPlacement();
                }
            }
        }
    }
}
