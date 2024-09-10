using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.InteropServices;



public class CohereIntegration : MonoBehaviour
{
    private string apiUrl = "https://api.cohere.ai/v1/generate";
    private string apiKey = "iUQfod3NXl7FzpL5Xj79ao9Pu8JYyVgMPfv2pRwJ";  
    private string master_prompt = "From the provided journal, give me a list of objects to be included in my 3D game that designed for user to explore their own stories reflected in the provided journal. Be concise and only list out the objects and their short description.";
    private string cohere_prompt;

    public IEnumerator SendJournalToCohere(string journalText, System.Action<string> callback)
    {
        cohere_prompt = $"User's journal: \"{journalText}\". {master_prompt}";


        var jsonData = new
        // Need to fine tune
        {
            model = "command-xlarge-nightly", 
            prompt = cohere_prompt,
            max_tokens = 1000
        };
        Debug.Log("prompt = " + cohere_prompt);

        string jsonBody = JsonConvert.SerializeObject(jsonData);
        Debug.Log("Cohere JSON request body = " + jsonBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error in Cohere API request: " + request.error);
        }
        else
        {
            // Parse the JSON response
            JObject jsonResponse = JObject.Parse(request.downloadHandler.text);
            string object_list = jsonResponse["generations"][0]["text"]?.ToString();

            Debug.Log("Cohere Response Json: " + jsonResponse);

            callback(object_list);
        }
    }
}
