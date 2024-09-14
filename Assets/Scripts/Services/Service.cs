using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;

public class Service : MonoBehaviour
{
    private string envFilePath;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Define the path for .env file containing API keys
        envFilePath = Path.Combine(Application.streamingAssetsPath, ".env");
    }

    // Method to read API key from the .env file
    private string GetApiKey(string apiKeyName)
    {
        if (!File.Exists(envFilePath))
        {
            Debug.LogError(".env file not found!");
            return null;
        }

        // Load and parse the .env file
        string[] envLines = File.ReadAllLines(envFilePath);
        Dictionary<string, string> envVars = new Dictionary<string, string>();

        // Parse each line to populate environment variables
        foreach (string line in envLines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) // Skip empty lines and comments
            {
                continue;
            }

            string[] parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                envVars[key] = value;
            }
        }

        // Return the requested API key value
        return envVars.ContainsKey(apiKeyName) ? envVars[apiKeyName] : null;
    }

    // GET Request
    public IEnumerator GetRequest(string apiKeyName, string url)
    {
        string apiKey = GetApiKey(apiKeyName);
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError($"API key for {apiKeyName} not found.");
            yield break;
        }

        // Append API key as query parameter to the URL
        UnityWebRequest request = UnityWebRequest.Get($"{url}?apiKey={apiKey}");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
    }

    // POST Request
    public IEnumerator PostRequest(string apiKeyName, string url, object jsonData)
    {
        string apiKey = GetApiKey(apiKeyName);
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError($"API key for {apiKeyName} not found.");
            yield break;
        }

        // Serialize the JSON object
        string jsonPayload = JsonUtility.ToJson(jsonData);

        // Create UnityWebRequest for POST with JSON data
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
    }
}
