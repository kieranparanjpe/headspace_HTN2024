using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JournalManager : MonoBehaviour
{
    public Button submitButton; //journal submit button
    public CohereIntegration cohereIntegration;
    public MeshyService meshyService;
    private string input;


    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitJournal);

    }

    public void OnSubmitJournal()
    {
        input = GameObject.Find("JournalField").GetComponent<UnityEngine.UI.InputField>().text;
        StartCoroutine(cohereIntegration.SendJournalToCohere(input, OnJournalProcessed));
    }

    private void OnJournalProcessed(string[] objects)
    {
        StartCoroutine(ProcessObjectsWithDelay(objects));
    }

    /*  Attempt to fix Error: HTTP/1.1 429 Too Many Requests
        Cause: As looping through objs directly, immediate Meshy API call
    */
    private IEnumerator ProcessObjectsWithDelay(string[] objects)
    {
        foreach (var obj in objects)
        {
            Debug.Log("Processing Object --- " + obj);
            StartCoroutine(meshyService.MakeApiCall(obj)); 

            // Wait for 5 seconds before processing the next object
            yield return new WaitForSeconds(5f);
            // CreateObjectInWorld(obj);
        }
    }
    private void CreateObjectInWorld(string objName)
    {
        // Logic to instantiate a corresponding 3D object in the world based on objName
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{objName}");
        if (prefab != null)
        {
            Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Prefab for {objName} not found!");
        }
    }

    Vector3 GetRandomPosition()
    {
        // Returns a random position in the world to place the object
        return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    }
}
