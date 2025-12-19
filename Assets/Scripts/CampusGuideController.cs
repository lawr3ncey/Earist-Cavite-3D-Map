using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampusGuideController : MonoBehaviour
{
    // UI Elements
    public TMP_InputField fromInput;
    public TMP_InputField toInput;
    public Button findPathButton;
    public TextMeshProUGUI errorMessageText;


    // Reference to LocationManager and PathRenderer
    public LocationManager locationManager;
    public PathRenderer pathRenderer;

    void Start()
    {
        // Add a listener to the Find Path button
        findPathButton.onClick.AddListener(HandleFindPath);
    }

    void HandleFindPath()
    {
        string fromLocation = fromInput.text;
        string toLocation = toInput.text;

        Vector3 fromPos = locationManager.GetLocationPosition(fromLocation);
        Vector3 toPos = locationManager.GetLocationPosition(toLocation);

        if (fromPos != Vector3.zero && toPos != Vector3.zero)
        {

            if (fromPos != Vector3.zero && toPos != Vector3.zero)
            {
                // Valid locations, hide error message and draw path
                HideInvalidLocationMessage();

                // Clear any existing path
                pathRenderer.ClearPath();

                // Assign positions and draw path between "From" and "To" locations
                pathRenderer.DrawPath(fromPos, toPos);
            }
        }
        else
        {
            // Invalid locations, show error message
            ShowInvalidLocationMessage();
        }
    }

    private void ShowInvalidLocationMessage()
    {
        // Show the invalid location message when necessary
        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(true);
            errorMessageText.text = "Invalid Locations. Please try again.";
        }
    }

    private void HideInvalidLocationMessage()
    {
        // Hide the invalid location message when input is valid
        if (errorMessageText != null)
        {
            errorMessageText.gameObject.SetActive(false);
        }
    }

}
