using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    private SloydService sloydService;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private const string envFile = "Assets/local.env";
    private string test_journal = "Today I ate 3 chicken legs. I got the oil grease on my laptop keyboard, and my hackathon event is about to end. I have not got much things done yet.";

    void Start()
    {
        DotEnvLoader.LoadEnvFile(envFile);
        sloydService = new SloydService(this);
        GroqService groqService = new GroqService(this);

        groqService.GetResponse(test_journal, SpawnObjects);
        
    }

    public void SpawnObjects(List<GroqObject> objects) 
    {
        if (objects == null)
        {
            Debug.LogError("Failed to receive objects from GroqService.");
            return;
        }

        foreach (var obj in objects)
        {
            // Convert float arrays to Vector3
            Vector3 position = new Vector3(obj.position[0], obj.position[1], obj.position[2]);
            Vector3 rotation = new Vector3(obj.rotation[0], obj.rotation[1], obj.rotation[2]);

            sloydService.GetObject($"{obj.name}: {obj.description}", o => NewObject(o, position, rotation));

        }
    }

    public void NewObject(GameObject obj, Vector3 position, Vector3 rotation)
    {
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.eulerAngles = rotation;
        
        spawnedObjects.Add(obj);
    }

}
