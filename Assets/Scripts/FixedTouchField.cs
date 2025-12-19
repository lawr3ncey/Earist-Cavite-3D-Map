using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public Vector2 TouchDist;
    [HideInInspector] public Vector2 PointerOld;
    [HideInInspector] protected int PointerId;
    [HideInInspector] public bool Pressed;

    private bool isInteractingWithUI = false; // Tracks UI interaction

    void Update()
    {
        if (Pressed)
        {
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                if (!IsPointerOverUI(PointerId))
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    isInteractingWithUI = true; // Prevent camera movement
                }
            }
            else
            {
                if (!IsPointerOverUI())
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
                else
                {
                    isInteractingWithUI = true; // Prevent camera movement
                }
            }
        }
        else
        {
            TouchDist = Vector2.zero;
        }

        if (!Pressed)
        {
            isInteractingWithUI = false; // Reset UI interaction status
        }

        HandleRaycast();
    }

    private void HandleRaycast()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (IsPointerOverUI())
                return;

            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

            // Combine layers for raycasting
            int environmentLayers = LayerMask.GetMask("Ground", "Buildings", "Walls", "Floor_1", "Stairs", "Floor_2", "Stairs_2", "Floor_3", "Roof");

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, environmentLayers))
            {
                Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        PointerId = eventData.pointerId;
        PointerOld = eventData.position;
        isInteractingWithUI = IsPointerOverUI(PointerId); // Check UI interaction at touch start
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        isInteractingWithUI = false; // Reset when touch ends
    }

    private bool IsPointerOverUI(int fingerId = -1)
    {
        if (fingerId == -1) // Mouse input
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        else // Touch input
        {
            return EventSystem.current.IsPointerOverGameObject(fingerId);
        }
    }

    public bool IsInteractingWithUI()
    {
        return isInteractingWithUI;
    }
}
