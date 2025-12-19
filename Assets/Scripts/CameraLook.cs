using UnityEngine;
using UnityEngine.EventSystems;

public class CameraLook : MonoBehaviour
{
    private float XRotation; // Tracks the camera's vertical rotation
    [SerializeField] private Transform PlayerBody; // Reference to the player's body for horizontal rotation
    public float Sensivity = 40f;

    public FixedTouchField touchField; // Reference to FixedTouchField

    void Update()
    {
        // Skip camera movement if interacting with UI
        if (touchField != null && touchField.IsInteractingWithUI())
            return;

        // Skip input if pointer is over UI
        if (IsPointerOverUI()) return;

        // Get input delta and rotate the camera
        Vector2 inputDelta = GetInputDelta();
        RotateCamera(inputDelta);
    }

    private Vector2 GetInputDelta()
    {
        Vector2 delta = Vector2.zero;

        if (Input.GetMouseButton(1)) // Right-click to look around
        {
            delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * Sensivity * Time.deltaTime;
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                delta = touch.deltaPosition * Sensivity * Time.deltaTime;
            }
        }

        return delta;
    }

    private void RotateCamera(Vector2 delta)
    {
        PlayerBody.Rotate(Vector3.up * delta.x); // Horizontal rotation
        XRotation -= delta.y; // Vertical rotation
        XRotation = Mathf.Clamp(XRotation, -90f, 90f); // Prevent flipping
        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
    }

    private bool IsPointerOverUI()
    {
        if (Input.touchCount > 0)
        {
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }
        return EventSystem.current.IsPointerOverGameObject();
    }
}
