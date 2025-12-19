using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NLPManager : MonoBehaviour
{
    public Transform player; // Assign your first-person controller's Transform in the Inspector

    // Test phrases
    public void TestPhraseDetection()
    {
        // List of phrases with [location] placeholder
        string[] testPhrases = {
            "How to get to [location]", "Where is [location]", "Show me the way to [location]",
            "Find [location]", "Take me to [location]", "Navigate [location]",
            "Find the shortest path to [location]", "Reach [location]",
            "What's the best way to get to [location]", "How can I reach [location]",
            "Where can I find [location]", "Can you lead me to [location]", "What’s the route to [location]",
            "Where exactly is [location]", "Can you direct me to [location]", "How far is [location] from here",
            "Give me directions to [location]", "What's the quickest way to [location]",
            "How do I navigate to [location]", "Please guide me to [location]",
            "What’s the nearest way to [location]", "How do I get from here to [location]",
            "Where do I go to find [location]", "Take me to the nearest [location]",
            "Could you help me find my way to [location]", "Give me a route to [location]",
            "What's the distance to [location] from here", "How long does it take to get to [location] from here",
            "How many miles/kilometers is [location] from here", "What’s the travel time to [location] from here",
            "How far away is [location] from here", "How far is it to [location] from here",
            "What's the distance between here and [location]", "How much time does it take to get to [location] from here",
            "How distant is [location] from here", "What’s the quickest route to [location] from here",
            "How far is [location] from this spot", "Is [location] close to here",
            "How far do I have to go to reach [location]", "What’s the nearest route to [location] from here",
            "Find the quickest route to [location]", "Show me the fastest way to [location]",
            "Give me the most direct route to [location]", "Find the most efficient way to [location]",
            "Take me via the shortest route to [location]", "Find the quickest way to [location]",
            "Show me the most direct path to [location]", "Navigate me to [location] along the fastest route",
            "Find the fastest path to [location]", "Give me the best shortcut to [location]",
            "What’s the fastest way to get to [location]", "Take me the quickest way to [location]",
            "Find the most time-saving route to [location]", "Give me the least time-consuming path to [location]",
            "Show me the route with the least travel time to [location]", "Find the safest route to [location]",
            "Show me the safest way to [location]", "Navigate me to [location] along the safest path",
            "Find the most secure way to [location]", "Take me on the safest route to [location]",
            "Find the safest road to [location]", "Show me the safest travel option to [location]",
            "What’s the safest path to [location]", "Give me the most secure way to get to [location]",
            "Guide me to [location] via the safest route", "Find the least risky path to [location]",
            "Show me the safest streets to [location]", "What’s the safest route I can take to [location]",
            "Give me directions to [location] using the safest path", "Find the safest way to travel to [location]",
            "Connect me to [location]", "Link me to [location]", "Put me in touch with [location]",
            "Help me get in contact with [location]", "Direct me to [location]", "Put me through to [location]",
            "Transfer me to [location]", "Establish a connection to [location]", "Connect me with [location]",
            "Can you reach out to [location] for me", "Put me in touch with someone at [location]",
            "Get me through to [location]", "Patch me through to [location]", "I need to speak with someone at [location]",
            "Link me up with [location]", "Connect me with the team at [location]", "Guide me to [location]",
            "Take me to [location]", "Show me the way to [location]", "Direct me to [location]",
            "Show me how to get to [location]", "Escort me to [location]", "Walk me to [location]",
            "Take me to the [location]", "Show me the route to [location]", "Navigate me to [location]",
            "Point me in the direction of [location]", "Lead me towards to [location]", "Help me find my way to [location]",
            "Take me along the path to [location]",

    // New walking-related phrases
    "How do I get to [location] by walking?", "Where is [location] on foot from here?", "Show me the walking route to [location].",
    "Find the path to [location] on foot.", "Take me to [location] by walking.", "Navigate me to [location] on foot.",
    "Find the shortest walking path to [location].", "Reach [location] on foot.", "What’s the best way to walk to [location]?",
    "How can I walk to [location] from here?", "Where do I go on foot to reach [location]?", "Can you lead me to [location] by walking?",
    "What’s the quickest route to walk to [location]?", "Where exactly is [location] by walking?", "Can you direct me to [location] by foot?",
    "How far is [location] on foot from here?", "Give me walking directions to [location].", "What's the fastest walking way to [location]?",
    "How do I walk to [location] from here?", "Please guide me to [location] by walking.", "What’s the nearest way to walk to [location]?",
    "How do I get to [location] walking from here?", "Where can I find [location] on foot?", "Take me to the nearest [location] by walking.",
    "Can you help me find my way to [location] on foot?", "Give me a walking route to [location].", "What's the distance to [location] if I walk?",
    "How long does it take to walk to [location] from here?", "How many minutes does it take to walk to [location]?", "What’s the walking time to [location] from here?",
    "How far is it to [location] on foot from this spot?", "How long does it take to walk to [location] from here?", "How far is [location] from here walking?",
    "How distant is [location] from here by foot?", "What's the shortest walking distance to [location]?", "How long would it take me to walk to [location]?",
    "What’s the quickest walking path to [location] from here?", "How far do I have to walk to reach [location]?", "What’s the best walking route to [location]?",
    "How can I walk to [location] without getting lost?", "Can you find the quickest way to walk to [location]?", "Show me the quickest walking path to [location].",
    "What’s the best route to get to [location] on foot?", "How do I find [location] on foot from here?", "Can you show me the shortest walking route to [location]?",
    "What’s the best way to walk to [location] from here?", "Where is [location] by walking from here?", "How do I walk to [location] without getting lost?",
    "What’s the best walking direction to [location]?", "How do I get to [location] on foot through the shortest path?", "Can you walk me to [location]?",
    "Where exactly is [location] by walking?", "How far is it to walk to [location] if I walk straight?", "Show me the fastest walking way to [location].",
    "Can you lead me to [location] by walking the fastest route?", "How do I get to [location] without walking through the park?", "What’s the shortest walking route to [location]?",
    "Can you guide me to [location] without detours?", "How can I reach [location] walking the quickest way?", "Where is the most direct walking route to [location]?",
    "What's the best walking path to [location] avoiding traffic?", "How do I walk to [location] without passing through crowded streets?", "Can you help me find the quickest walking path to [location]?",
    "How far is [location] on foot from here?", "Take me along the shortest walking path to [location].", "How do I get to [location] walking without a map?",
    "Can you show me a straight walk to [location]?", "What’s the most direct walking path to [location]?", "Where is [location] by walking in the most straightforward way?",
    "Can you show me how to walk to [location] directly?", "How do I get to [location] without taking a detour on foot?", "What’s the easiest walking route to [location] from here?",
    "How do I find [location] on foot from here without backtracking?", "Can you find the quickest footpath to [location]?", "Show me how to walk directly to [location].",
    "How do I get to [location] with the least walking distance?", "Can you suggest the quickest walk to [location]?", "What’s the walking path to [location] that avoids hills?",
    "Can you show me the shortest walking path to reach [location]?", "What’s the most efficient walking route to [location]?", "How do I reach [location] by walking with the least effort?",
    "How far away is [location] on foot?", "Where is [location] walking the most direct route from here?", "What’s the best way to reach [location] by walking?",
    "How do I navigate to [location] without any unnecessary detours?", "Can you guide me to [location] by walking with the fewest turns?", "Where can I walk to reach [location] in the shortest time?",
    "How far is [location] from here if I walk the most direct route?", "What’s the shortest walking route to get to [location]?", "Can you help me find the quickest walking path to [location]?",
    "Where is [location] by walking, avoiding busy areas?", "How do I walk to [location] without passing any high-traffic places?", "Take me the quickest way on foot to [location].",
    "Where is [location] on foot from here avoiding all busy streets?", "How do I reach [location] without walking through congested spots?", "Can you guide me through the fastest footpath to [location]?",
    "Where is the closest walking path to [location]?", "How do I get to [location] walking with the fewest interruptions?", "How do I find [location] walking from here without making unnecessary stops?",
    "What’s the most efficient walking route to [location] from here?", "How can I walk to [location] while avoiding crowded areas?", "What’s the best route to walk to [location] from here?",
    "How do I walk to [location] without wasting time?", "How do I reach [location] on foot through the most convenient route?", "Where should I walk to reach [location] directly?",
    "Can you show me the fastest walking path to [location]?", "What’s the most time-saving walking route to [location]?", "How do I get to [location] walking the most efficient way?",
    "Where is the closest walking path to [location] from here?", "What’s the easiest way to walk to [location]?", "Can you guide me along the quickest footpath to [location]?",
    "How do I navigate to [location] avoiding unnecessary walking?", "What’s the fastest way to walk to [location] without taking the long way?", "Can you help me walk directly to [location]?",
    "How far is [location] walking from here if I take the shortest path?", "What’s the easiest walking direction to [location]?", "Where should I go on foot to reach [location]?",
    "Can you show me a direct walking path to [location]?", "How do I get to [location] walking with the shortest time?", "What’s the most convenient walking route to [location]?",
    "How do I walk to [location] by avoiding major streets?", "Where is [location] on foot, avoiding highways?", "How do I get to [location] without walking through busy places?",
    "Can you guide me along the safest path to [location] on foot?", "What’s the quickest route to walk to [location]?", "How do I get to [location] walking via the shortest path?",
    "How do I find the fastest walking way to [location]?", "Where is the most efficient footpath to [location]?", "What’s the safest walking route to [location]?",
    "Can you help me walk to [location] without making unnecessary turns?", "How far is [location] by walking without unnecessary detours?", "What’s the most direct route to walk to [location]?",
    "How do I reach [location] walking the shortest distance possible?", "Where can I walk to reach [location] avoiding busy streets?", "Can you guide me to [location] with the least amount of walking?",
    "What’s the most time-efficient walking path to [location]?", "How far is [location] from here on foot if I take the shortest route?", "Can you help me get to [location] on foot without any backtracking?",
    "Where do I walk to reach [location] directly?", "How do I get to [location] by walking the easiest way?", "Can you show me the fastest way to get to [location] on foot?",
    "What’s the best walking route to [location] avoiding detours?", "How do I walk to [location] in the least amount of time possible?", "How do I reach [location] without taking unnecessary paths?",
    "What’s the easiest way to get to [location] walking?", "Can you guide me along the most direct walking path to [location]?", "Where should I go walking to reach [location] directly?",
    "How do I find the shortest walking route to [location]?", "What’s the best way to walk to [location] avoiding high-traffic roads?", "How do I navigate to [location] walking the shortest possible way?",
    "Can you show me the best walking route to get to [location]?", "Where is [location] walking without taking detours?", "How do I reach [location] walking without passing through crowded spots?",
    "How do I get to [location] walking without taking any unnecessary routes?", "Can you help me find the shortest walking path to [location] from here?", "Where can I walk to get to [location] without extra steps?",
    "How do I get to [location] on foot by the most direct route?", "What’s the quickest walking route to [location] from here?", "Can you guide me along the shortest walking way to [location]?",
    "How far is it to walk to [location] via the most efficient path?", "What’s the quickest way to walk to [location] from here?", "Can you help me get to [location] without walking on busy streets?",
    "How do I walk to [location] in the shortest time possible?", "Where is [location] walking the fastest path from here?", "Can you show me a route to [location] walking the quickest way possible?",
    "How far is [location] walking from here avoiding any long detours?", "What’s the most time-efficient walking path to [location]?", "Can you guide me along the most straightforward walking path to [location]?",
    "How do I get to [location] on foot with the least walking distance?", "Where is [location] if I walk the most direct path?", "What’s the best route to walk to [location] without any extra turns?",
    "How do I get to [location] walking, avoiding hills or obstacles?", "Can you show me the quickest walking route to get to [location] with no detours?", "How far is [location] from here by foot without any backtracking?",
    "How do I reach [location] walking the easiest path?", "Can you help me walk to [location] using the shortest route possible?", "What’s the shortest way to walk to [location] from here?",
    "Where is [location] if I walk via the most direct route?", "How far is it to walk to [location] without making any wrong turns?", "Can you guide me to [location] walking the quickest possible way?",
    "How do I walk to [location] in the least amount of time?", "What’s the best way to walk to [location] avoiding busy streets?", "Where is the best place to start walking to reach [location]?",
    "How do I get to [location] walking via the shortest and safest route?", "Can you help me find the best route to walk to [location]?", "How do I walk to [location] using the least time-consuming path?",
    "What’s the most efficient walking route to reach [location]?", "Can you show me a way to walk to [location] avoiding the busy areas?", "Where should I walk to find [location] easily?",
    "How do I navigate to [location] walking, avoiding obstacles?", "What’s the best walking route to take to reach [location] directly?", "How do I reach [location] on foot with the least effort?",
    "How far is [location] if I walk from here avoiding busy streets?", "Can you show me the fastest way to walk to [location] from here?", "Where can I walk to get to [location] without extra detours?",
    "How do I walk to [location] without any backtracking or detours?", "Can you guide me to [location] using the most direct walking route?", "Where is the shortest path to walk to [location]?",
    "How do I get to [location] walking the easiest way possible?", "Can you help me find a walking path to [location] that avoids busy roads?", "What’s the quickest walking route to [location] that avoids traffic?",
    "How far away is [location] by walking from here if I take the direct route?", "Where do I walk to get to [location] as quickly as possible?", "How do I reach [location] without walking through crowded spots?",
    "Can you help me get to [location] walking the shortest path?", "What’s the fastest footpath to reach [location]?", "How do I find the easiest walking path to [location]?",
    "Where is [location] walking via the quickest route?", "How do I walk to [location] by the most efficient means?", "Can you show me the most direct walking way to [location] without detours?",
    "How far is it to walk to [location] using the shortest possible route?", "Where should I go to find [location] walking?", "How do I walk to [location] avoiding high-traffic areas?",
    "What’s the fastest way to reach [location] by walking?", "How do I walk to [location] in the least amount of time and effort?", "Can you guide me to [location] on foot by the easiest route?",
    "Where is the best walking route to [location]?", "How do I find [location] by walking through the shortest path?", "Can you show me a walking route to [location] with the least time?",
    "Where is the quickest walking path to [location] from here?", "How do I walk to [location] through the least complicated path?", "What’s the easiest walking route to reach [location] from here?",
    "Can you guide me to [location] via the least time-consuming walking route?", "How do I reach [location] without walking on crowded streets?", "How far is it to walk to [location] avoiding any busy intersections?",
    "Where is the fastest way to walk to [location] from here?", "Can you show me a walking route to [location] without extra turns?", "How do I walk to [location] through the safest path available?",
    "What’s the best walking path to reach [location] avoiding heavy traffic?", "Can you help me find the shortest walking route to [location] from here?", "How do I navigate to [location] by walking, avoiding crowded areas?",
    "What’s the best way to get to [location] walking without any detours?", "How do I find [location] walking the easiest and quickest way?", "Where is [location] if I walk the shortest route possible?",
    "How do I reach [location] walking while avoiding busy areas?", "Can you help me navigate to [location] by foot without getting lost?", "Where is the most direct walking route to [location] from here?",
    "How do I get to [location] by walking via the shortest and safest path?", "Can you guide me to [location] using the most efficient walking route?", "What’s the best way to walk to [location] without taking the long route?",
    "How far is [location] from here by walking via the most direct path?", "Where can I walk to find [location] avoiding heavy traffic?", "How do I get to [location] on foot by walking through the safest path?",
    "Can you show me how to walk to [location] without making extra steps?", "How do I find [location] by walking the shortest route possible?", "What’s the easiest way to get to [location] walking from here?",
    "Where is the closest footpath to reach [location] from here?", "How do I get to [location] walking using the least time-consuming route?", "Can you help me find a direct walking path to [location]?",
    "What’s the fastest walking route to [location] avoiding detours?", "How do I get to [location] walking, avoiding any busy places?", "Can you guide me to [location] by foot with the least effort?",
    "What’s the safest and quickest way to walk to [location]?", "How far is it to reach [location] on foot if I take the shortest path?", "Can you help me find the best walking route to [location] directly?",
    "Where do I walk to get to [location] without unnecessary turns?", "How do I walk to [location] avoiding any unnecessary detours?", "What’s the most efficient walking route to take to [location]?",
    "Where should I walk to reach [location] by the quickest path?", "How do I navigate to [location] on foot with minimal detours?", "Can you guide me through the best walking path to [location]?",
    "What’s the quickest way to walk to [location] from here?", "Where do I go on foot to get to [location] in the least time?", "How do I find the safest and fastest route to walk to [location]?",
    "What’s the fastest walking path to [location] with no extra turns?", "How do I get to [location] walking, avoiding long detours?", "Can you show me the best route to walk to [location] from here?",
    "How far is it to walk to [location] via the most direct path?", "Where is [location] walking from here, without any backtracking?", "How do I walk to [location] with the least amount of time?",
    "Can you guide me to [location] with the quickest footpath?", "How do I get to [location] walking with the least detours?", "What’s the most direct walking route to [location] from here?",
    "How do I navigate to [location] walking the fastest route?", "Can you help me walk to [location] via the most efficient route?", "What’s the best walking path to take to get to [location]?",
    "How far is it to walk to [location] from here avoiding busy roads?", "Can you show me the most direct way to reach [location] on foot?", "How do I walk to [location] without taking any unnecessary detours?",
    "Can you guide me to [location] via the shortest possible walking path?", "Where do I go to reach [location] by walking in the fastest time?", "What’s the easiest route to walk to [location] from here?",
    "How far is [location] walking via the shortest path possible?", "Can you guide me along the best route to walk to [location]?", "What’s the best footpath to get to [location] quickly?",
    "How do I get to [location] walking in the least amount of time?", "Can you help me find the safest walking path to [location]?", "Where should I walk to reach [location] directly?",
    "How do I find [location] walking with no extra steps?", "Can you show me the fastest way to walk to [location]?", "What’s the most efficient route to walk to [location]?",
    "Where is the shortest walking route to [location] from here?", "How far away is [location] by walking from here without detours?", "How do I get to [location] by walking the most direct way possible?",
    "Can you guide me to [location] by walking without taking the longer way?", "What’s the easiest walking way to get to [location]?", "How do I find the quickest path to walk to [location]?",
    "Where should I walk to find [location] with the shortest distance?", "Can you guide me to [location] without walking extra?", "What’s the fastest walking path to get to [location]?",
    "How do I reach [location] walking the most direct way?", "How do I find the best route to walk to [location]?", "Can you guide me to [location] with the least amount of walking?"


        };

        // List of room names (locations)
        List<string> roomNames = new List<string>
        {
            "Criminology Building", "Canteen", "Covered Court", "Stage", "Basketball Court",
        "Administration Office", "Registrars Office", "Speech Laboratory", "Internet Room",
        "Library Undergrad Program", "Graduate Office", "Room 302", "Computer Laboratory",
        "MPH Room 1", "MPH Room 2", "MPH Room 3", "Pent House", "Faculty Room",
        "Physics Laboratory", "Psychology Laboratory", "Chemistry Laboratory",
        "Library Graduate Program", "Room 203", "Head Quarters", "Room 101", "Room 102",
        "Room 103", "Room 104", "Room 105", "Room 106", "Room 107", "Room 204",
        "Room 205", "Room 206", "Room 207", "Room 208", "Room 209", "Room 210",
        "Room 211", "Comfort Room", "Annex 101", "Annex Building", "Annex 201", "Annex 301", "Annex 102",
        "Annex 202", "Annex 302", "Annex 103", "Annex 203", "Annex 303", "Conference Room",
        "School Clinic", "Dragon Fruit Farm", "Evacuation Spot A", "Evacuation Spot B", "Fire Exit",
        "Fire Exit", "Fire Exit 1", "Fire Exit 2", "Fire Exit 3", "Fire Exit 4", "Fire Exit 5",

        "Main Building", "Entrance"
        };

        // Loop through all phrases and room names to generate test cases
        foreach (string phrase in testPhrases)
        {
            foreach (string room in roomNames)
            {
                // Replace [location] with the room name
                string testPhrase = phrase.Replace("[location]", room);

                // Call your method to extract the location from the phrase
                string detectedLocation = ExtractLocationFromPhrase(testPhrase);

                
            }
        }
    }

    private const string API_KEY = "YOUR_API_KEY";
    private const string API_URL = "https://language.googleapis.com/v1/documents:analyzeEntities?key=";

    // Dictionary mapping keywords and synonyms to canonical location names
    private Dictionary<string, string> synonyms = new Dictionary<string, string>()
    {
        { "entrance", "entrance" },
        { "gate", "entrance" },
        { "main entrance", "entrance" },
        { "main building", "main building" },
        { "main", "main building" },
        { "annex building", "annex building" },
        { "annex", "annex building" },
    };

    public void AnalyzeText(string text)
    {
        Debug.Log("AnalyzeText called with input: " + text);
        StartCoroutine(SendNLPRequest(text));
    }

    private IEnumerator SendNLPRequest(string text)
    {

        Debug.Log("Starting NLP request...");
        // Check for empty or null input
        if (string.IsNullOrWhiteSpace(text))
        {
            Debug.LogError("Input text is null or empty.");
            yield break;
        }

        // Continue coroutine logic
        Debug.Log("NLP request continuing...");



        // Create the request object
        NLPRequest nlpRequest = new NLPRequest
        {
            document = new NLPDocument
            {
                content = text // Set the text content here
            }
        };

        // Serialize the request object to JSON
        string jsonRequest = JsonUtility.ToJson(nlpRequest);

        // Log the JSON payload for debugging
        Debug.Log("JSON Payload: " + jsonRequest);

        // Create and configure the UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(API_URL + API_KEY, "POST");
        byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(jsonRequest);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Handle the API response
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("NLP Response: " + request.downloadHandler.text);
            ProcessNLPResponse(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Response Code: " + request.responseCode);
            Debug.LogError("Response Body: " + request.downloadHandler.text);
        }
    }

    private void ProcessNLPResponse(string jsonResponse)
    {
        Debug.Log("Processing NLP Response...");
        Debug.Log($"Raw NLP Response: {jsonResponse}");
        NLPResponse response = JsonUtility.FromJson<NLPResponse>(jsonResponse);

        foreach (var entity in response.entities)
        {
            Debug.Log($"Entity: {entity.name}, Type: {entity.type}, Salience: {entity.salience}");

            // Process LOCATION entities directly
            if (entity.type == "LOCATION")
            {
                string matchedLocation = ExtractLocationFromPhrase(entity.name);
                if (!string.IsNullOrEmpty(matchedLocation))
                {
                    PerformActionBasedOnEntity(matchedLocation);
                    return; // Exit after the first match
                }
            }
        }

        Debug.LogWarning("No recognized location in the NLP response.");
        DisplayFeedbackToUser("Sorry, I couldn't identify a location. Please try again.");
    }

    private void DisplayFeedbackToUser(string message)
    {
        Debug.Log($"Feedback: {message}");
        // Example: Update a UI Text element
        var feedbackText = GameObject.Find("FeedbackText").GetComponent<UnityEngine.UI.Text>();
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
    }

    private void PerformActionBasedOnEntity(string location)
    {
        // Handle recognized locations and trigger path visualization
        string canonicalLocation = null;

        if (synonyms.TryGetValue(location.ToLower(), out canonicalLocation))
        {
            GameObject[] locationObjects = GameObject.FindGameObjectsWithTag("Location");
            foreach (var obj in locationObjects)
            {
                if (obj.name.Equals(canonicalLocation, System.StringComparison.OrdinalIgnoreCase))
                {
                    Vector3 destination = obj.transform.position;
                    // Prepare the path array and send it to PathVisualizer
                    Vector3[] path = new Vector3[2];
                    path[0] = player.position; // Player's position
                    path[1] = destination;     // Destination position
                }
            }
        }
    }

    private string ExtractLocationFromPhrase(string phrase)
    {
        // Normalize text: lowercase, trim, and remove punctuation
        phrase = phrase.ToLower().Trim();
        phrase = Regex.Replace(phrase, @"[^\w\s]", ""); // Remove punctuation


        return null; // Return null if no match found
    }

    void Start()
    {
        Debug.Log("NLPManager script initialized.");
        TestPhraseDetection();
    }
}



// Ensure these are in the same script (NLPManager.cs) or another script that's included

[System.Serializable]
public class Entity
{
    public string name;       // Name of the entity
    public string type;       // Type of the entity (e.g., LOCATION)
    public float salience;    // Relevance of the entity
}

[System.Serializable]
public class NLPResponse
{
    public Entity[] entities; // Array of entities in the response
}

[System.Serializable]
public class NLPDocument
{
    public string type = "PLAIN_TEXT";
    public string content; // Holds the text to be analyzed
}

[System.Serializable]
public class NLPRequest
{
    public NLPDocument document = new NLPDocument();
    public string encodingType = "UTF8";
}