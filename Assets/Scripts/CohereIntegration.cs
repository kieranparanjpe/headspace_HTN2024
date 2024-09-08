using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;  
using System.Text;

public class CohereIntegration : MonoBehaviour
{
    private string apiUrl = "https://api.cohere.ai/v1/generate";
    private string apiKey = "iUQfod3NXl7FzpL5Xj79ao9Pu8JYyVgMPfv2pRwJ";  

    public IEnumerator SendJournalToCohere(string journalText, System.Action<string> callback)
    {
        var jsonData = new
        {
            model = "command-xlarge-nightly", // or the model you want to use
            prompt = journalText,
            max_tokens = 100 // or any other parameter you need
        };

        string jsonBody = JsonConvert.SerializeObject(jsonData);
        Debug.Log("cohere jsonBody = " + jsonBody);

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
            var jsonResponse = request.downloadHandler.text;
            callback(jsonResponse); 
        }
    }
}
