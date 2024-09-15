using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    private SloydService sloydService;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private const string envFile = "Assets/local.env";
    private string system_prompt_objects;

    private int postionIndex = 0;
    private string test_journal = "Today I ate 3 chicken legs. I got the oil grease on my laptop keyboard, and my hackathon event is about to end. I have not got much things done yet.";

    public EmotionMap emotionMap;
    void Awake()
    {
        emotionMap.SetEmotion("anger");
        DotEnvLoader.LoadEnvFile(envFile);
        sloydService = new SloydService(this);
        GroqService groqService = new GroqService(this);
        
        system_prompt_objects = "Your job is design a space which will reflect, support and complement the mood, content, emotional state and vibe of a given journal entry. The prompt you receive will be this journal entry. You will do this by generating a list of objects that might contribute to this environment. Each object should be cohesive, and make sense together with all the other objects. You will also need to determine the position of each object in 3D space using XYZ coordinates, where X is horizontal, Y is vertical, and Z is depth, and a unit of 1 corresponds to 1 meter. You will also return the orientation of the object using euler angles, with each value being between 0 and 360 degrees. You will return your answer in the JSON format as a list, with each list element having the following attributes: name:string, description:string, position:[float,float,float], rotation [float,float,float]. You will return only the json information, you message should contain nothing else."; 
        groqService.GetResponse(system_prompt_objects, test_journal, SpawnObjects);
        
    }

    public void SpawnObjects(List<GroqObject> objects) 
    {
        if (objects == null)
        {
            Debug.LogError("Failed to receive objects from GroqService.");
            return;
        }
        for (int i = 0; i < objects.Count; i++)
        {
            var obj = objects[i];
            // Convert float arrays to Vector3
            // Vector3 position = new Vector3(obj.position[0], obj.position[1], obj.position[2]);
            // Vector3 rotation = new Vector3(obj.rotation[0], obj.rotation[1], obj.rotation[2]);
            print($"spawning obj: {i}");
            sloydService.GetObject($"{obj.name}: {obj.description}", o => NewObject(o, i));
        }
    }

    public void NewObject(GameObject obj, int i)
    {
        print($"spawned object {i}");
        obj.SetActive(true);
        //obj.transform.position = position;
        //obj.transform.eulerAngles = rotation;
        
        spawnedObjects.Add(obj);
        emotionMap.SetObject(obj, postionIndex ++);
    }

}
