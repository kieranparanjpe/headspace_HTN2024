using UnityEngine;
using UnityEngine.UI;

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

    private void OnJournalProcessed(string objects)
    {
        meshyService.TestOut(objects);
        // Debug.Log("Cohere response: " + objects);



        // Process the objects array to generate 3D world content
        // foreach (var obj in objects)
        // {
        //     Debug.Log("Object to add: " + obj);
        //     // Example: Instantiate prefabs based on the returned object names
        //     CreateObjectInWorld(obj);
        // }
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
