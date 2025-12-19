using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    // Dictionary to store location names and their world positions
    private Dictionary<string, Vector3> locations = new();
    public static LocationManager Instance { get; private set; }

    public AstarPath astarPath;  // Reference to A* Pathfinding

    public VoiceAssistantOutput voiceAssistant;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        RegisterLocations();  // Register locations on start
    }

    // Register locations based on GameObjects tagged "Location"
    void RegisterLocations()
    {
        GameObject[] locationObjects = GameObject.FindGameObjectsWithTag("Location");

        foreach (GameObject location in locationObjects)
        {
            locations[location.name] = location.transform.position;
        }

        Debug.Log($"Registered {locations.Count} locations.");
    }
 


    // Get the position of a location by name
    public Vector3 GetLocationPosition(string locationName)
    {
        if (locations.ContainsKey(locationName))
        {
            return locations[locationName];
        }
        else
        {
            Debug.LogWarning($"Location '{locationName}' not found.");

            // Trigger voice output to speak the result
            if (voiceAssistant != null)
            {
                voiceAssistant.Speak($"Sorry, location '{locationName}' not found. Try change the format of the name");
            }
            else
            {
                Debug.LogError("VoiceAssistantOutput is not found in the scene.");
            }

            return Vector3.zero;
        }
    }
}
