using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Siccity.GLTFUtility;
using UnityEngine;

public class SloydService
{
    private MonoBehaviour obj;
    private string clientID = "";
    private string clientSecret = "";
    private const string apiRoute = "https://api.sloyd.ai/create";
    
    public SloydService(MonoBehaviour obj)
    {
        this.obj = obj;
        clientID = Environment.GetEnvironmentVariable("SLOYD_CLIENT_ID");
        clientSecret = Environment.GetEnvironmentVariable("SLOYD_CLIENT_SECRET");
    }
    
    public void GetObject(string prompt, System.Action<GameObject> callback)
    {
        SloydRequest req = new SloydRequest(prompt, clientID, clientSecret, "glb", "json");
        obj.StartCoroutine(Service.PostRequest(apiRoute, "", req,  o => { SaveGLB((SloydResponse)o, callback); }, typeof(SloydResponse)));
    }

    private void SaveGLB(SloydResponse response, System.Action<GameObject> callback)
    {
        byte[] modelData = Convert.FromBase64String(response.ModelData);
        Debug.Log("Model downloaded. Size: " + modelData.Length + " bytes");

        string folderPath = Application.dataPath + "/3D_Models/";

        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string filePath = folderPath + response.Name + ".glb";
        System.IO.File.WriteAllBytes(filePath, modelData);

        // Fabrication: Import the model into the scene 
        GameObject result = Importer.LoadFromFile(filePath);
        //result.transform.SetParent(parentTransform, false);
        result.SetActive(false);
        callback(result);
    }
}

public class SloydRequest
{
    public string Prompt;
    public string ClientId;
    public string ClientSecret;
    public string ModelOutputType;
    public string ResponseEncoding;

    public SloydRequest(string prompt, string clientId, string clientSecret, string modelOutputType, string responseEncoding)
    {
        Prompt = prompt;
        ClientId = clientId;
        ClientSecret = clientSecret;
        ModelOutputType = modelOutputType;
        ResponseEncoding = responseEncoding;
    }
}

public class SloydResponse
{
    public string InteractionId;
    public string Name;
    public int ConfidenceScore;
    public string ModelOutputType;
    public string ResponseEncoding;
    public string ModelData;
    public string ThumbnailPreviewExportType;
    public string ThumbnailPreview;
    
   // public SloydResponse(string interactionId, string name, string confidenceScore, string responseEncoding, string modelOutputType, string modelData, string thumbnailPreviewExportType, string thumbnailPreview)
}
