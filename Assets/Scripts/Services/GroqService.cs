using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GroqService
{
    private MonoBehaviour obj;
    private string clientID;
    private string clientSecret;
    private const string apiRoute = "https://api.groq.com/openai/v1/chat/completions";

    public GroqService(MonoBehaviour obj)
    {
        this.obj = obj;
        clientSecret = Environment.GetEnvironmentVariable("GROQ_CLIENT_SECRET");
    }

    public void GetResponse(string prompt, System.Action<string> callback)
    {
        // List<GroqMessage> messages = new List<GroqMessage>
        // {
        //     new GroqMessage { role = "user", content = prompt }
        // };

 
        GroqMessage[] messages = {new GroqMessage("user", "test"), };

        Debug.Log(messages);
        GroqRequest req = new GroqRequest(messages, "llama3-8b-8192");
        obj.StartCoroutine(Service.PostRequest(apiRoute, clientSecret, req, o => {SaveResponse((GroqResponse)o, callback); }, typeof(GroqResponse)));

    }

    private void SaveResponse(GroqResponse response, System.Action<string> callback)
    {
        if (response != null && response.choices != null && response.choices.Length > 0)
        {
            Debug.Log("Groq Response: " + response.choices[0].message.content);
            callback(response.choices[0].message.content);
        }
        else
        {
            Debug.LogError("Failed to get a valid response from Groq.");
            Debug.Log($"Grog Response = {response.choices}");
            callback(null);
        }
    }
}

[System.Serializable]
public class GroqMessage
{
    public string role;
    public string content;

    public GroqMessage(string role, string content){
        this.role = role;
        this.content = content;
    }


}

[System.Serializable]
public class GroqRequest
{
    public GroqMessage[] messages;
    public string model;

    public GroqRequest(GroqMessage[] messages, string model)
    {
        this.messages = messages;
        this.model = model;
    }
}



[System.Serializable]
public class GroqResponse
{
    public GroqChoice[] choices;
}

public class GroqChoice
{
    public GroqMessage message;
}
