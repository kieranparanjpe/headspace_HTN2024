using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GroqService
{
    private MonoBehaviour obj;
    private string clientID;
    private string clientSecret;
    private string system_prompt;

    private string user_prompt;

    private const string apiRoute = "https://api.groq.com/openai/v1/chat/completions";

    public GroqService(MonoBehaviour obj)
    {
        this.obj = obj;
        clientSecret = Environment.GetEnvironmentVariable("GROQ_CLIENT_SECRET");
    }

    public void GetObjects(string system_prompt, string user_prompt, System.Action<List<GroqObject>> callback)
    {
        GroqMessage[] messages = {new GroqMessage("system", system_prompt), new GroqMessage("user", user_prompt),};

        Debug.Log("Groq Request JSON = " + string.Join<GroqMessage>(", ", messages));
        GroqRequest req = new GroqRequest(messages, "llama3-8b-8192");

        obj.StartCoroutine(Service.PostRequest(apiRoute, clientSecret, req, o => { ParseResponse((GroqResponse)o, callback); }, typeof(GroqResponse)));
    }

    public void GetEmotion(string system_prompt, string user_prompt, System.Action<string> callback)
    {
        GroqMessage[] messages = { new GroqMessage("system", system_prompt), new GroqMessage("user", user_prompt) };

        GroqRequest req = new GroqRequest(messages, "llama3-8b-8192");

        obj.StartCoroutine(Service.PostRequest(apiRoute, clientSecret, req, o => { ParseEmotion((GroqResponse)o, callback); }, typeof(GroqResponse)));
    }

    private void ParseEmotion(GroqResponse response, System.Action<string> callback)
    {
        Debug.Log("Emotion " + response);    

        if (response != null && response.choices != null)
        {
            Debug.Log("Groq Response: " + response.choices[0].message.content);

            // Parse JSON response
            try
            {
                string content = response.choices[0].message.content;
                callback(content);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
                // callback(null);
            }
        }
        else
        {
            Debug.LogError("Failed to get a valid response from Groq.");
            Debug.Log($"Groq Response = {response.choices}");
            // callback(null);
        }
    }
    private void ParseResponse(GroqResponse response, System.Action<List<GroqObject>> callback)
    {
        if (response != null && response.choices != null && response.choices.Length > 0)
        {
            Debug.Log("Groq Response: " + response.choices[0].message.content);

            // Parse JSON response
            try
            {
                string jsonContent = response.choices[0].message.content;
                Debug.Log("Json COnent = "+jsonContent);
                List<GroqObject> objects = JsonUtility.FromJson<GroqObjectList>("{\"objects\":" + jsonContent + "}").objects;
                Debug.Log("objects = " + objects.Count);
                callback(objects);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
                // callback(null);
            }
        }
        else
        {
            Debug.LogError("Failed to get a valid response from Groq.");
            Debug.Log($"Groq Response = {response.choices}");
            // callback(null);
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


[System.Serializable]
public class GroqObject
{
    public string name;
    public string description;
    public float[] position;
    public float[] rotation;
}

[System.Serializable]
public class GroqObjectList
{
    public List<GroqObject> objects;
}
