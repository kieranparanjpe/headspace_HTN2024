using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Service
{
    // Method to read API key from the .env file

    // GET Request. URL must include API key
    public static IEnumerator GetRequest(string url, System.Action<object> callback, Type responseType)
    {
        // Append API key as query parameter to the URL
        UnityWebRequest request = UnityWebRequest.Get($"{url}");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
            callback(JsonUtility.FromJson(request.downloadHandler.text, responseType)); 
        }
    }

    // POST Request
    
    /*
    public static IEnumerator PostRequest(string apiKeyName, string url, object jsonData, System.Action<object> callback)
    {
        string apiKey = GetApiKey(apiKeyName);
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError($"API key for {apiKeyName} not found.");
            yield break;
        }

        // Serialize the JSON object
        string jsonPayload = JsonUtility.ToJson(jsonData);
        JsonUtility.FromJson()

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
    */
    
    
    public static IEnumerator PostRequest(string url, string apiKey, object jsonData, System.Action<object> callback, Type responseType)
    {
        
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
            callback(JsonUtility.FromJson(request.downloadHandler.text, responseType)); 
        }

    }
}
