using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    public List<GameObject> spawnedObjects = new List<GameObject>();
    private const string envFile = "Assets/local.env";

    void Awake()
    {
        DotEnvLoader.LoadEnvFile(envFile);
    }

    void Start()
    {
        SloydService sloydService = new SloydService(this);
        
        //sloydService.GetObject("cool looking box", o => NewObject(o, 0));
    }

    public void NewObject(GameObject obj, int n)
    {
        obj.SetActive(true);
        obj.transform.position = Vector3.zero;
        
        spawnedObjects.Add(obj);
    }
    
}
