using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Text;
using UnityEngine.UIElements;
using Siccity.GLTFUtility;

public class MeshyService : MonoBehaviour
{
    public Button apiButton;  // Assign the button in the inspector
    public GameObject objectPrefab;  // Assign the prefab that will be spawned
    private string apiUrl = "https://api.meshy.ai/v2/text-to-3d";  // Replace with the real API URL
    private string retrieveUrl = "https://api.meshy.ai/v2/text-to-3d/{0}";  // URL template for retrieving the model

    private string apiKey = "msy_LY0EIx84452SbWl82XNU8CnYLyfkq78GBxvZ";
    private string prompt = "A very big mouse";
    private string artStyle = "realistic";
    private string negativePrompt = "low quality, low resolution, low poly, ugly";
    public Transform parentTransform;

    void Start()
    {
        //apiButton.onClick.AddListener(OnApiButtonClick);  // Register button click event
        //StartCoroutine(MakeApiCall());
        StartCoroutine(RetrieveModel("0191d3c9-d59c-75c1-bb7c-4908ceee612b"));
    }

    // Function triggered when the button is clicked
    void OnApiButtonClick()
    {
        StartCoroutine(MakeApiCall());  // Start the API call coroutine
    }

    IEnumerator MakeApiCall()
    {
        JObject payload = new JObject
        {
            { "mode", "preview" },
            { "prompt", prompt },
            { "art_style", artStyle },
            { "negative_prompt", negativePrompt }
        };

        string jsonPayload = payload.ToString();

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            JObject responseObject = JObject.Parse(request.downloadHandler.text);
            string modelId = responseObject["result"]?.ToString();  // Extract model ID from response
            if (!string.IsNullOrEmpty(modelId))
            {
                Debug.Log(responseObject);
                // Start retrieving the model
                StartCoroutine(RetrieveModel(modelId));
            }
            else
            {
                Debug.LogError("Model ID not found in the response.");
            }
        }
    }

    IEnumerator RetrieveModel(string modelId)
    {
        string url = string.Format(retrieveUrl, modelId);
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        Debug.Log("Retrieving model...");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            JObject responseObject = JObject.Parse(request.downloadHandler.text);
            string glbUrl = responseObject["model_urls"]?["glb"]?.ToString();  // Extract the URL of the GLB model
            if (!string.IsNullOrEmpty(glbUrl))
            {
                // Start downloading the GLB model
                yield return StartCoroutine(DownloadModel(glbUrl));
            }
        }
    }

    IEnumerator DownloadModel(string modelUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(modelUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error downloading model: " + request.error);
        }
        else
        {
            // Handle the downloaded GLB file here
            byte[] modelData = request.downloadHandler.data;
            Debug.Log("Model downloaded. Size: " + modelData.Length + " bytes");

            string filePath = "C://dev/model.glb";
            System.IO.File.WriteAllBytes(filePath, modelData);

            GameObject result = Importer.LoadFromFile(filePath);
            result.transform.SetParent(parentTransform, false);
            result.transform.position = Vector3.zero; // Set position as needed

        }
    }
}