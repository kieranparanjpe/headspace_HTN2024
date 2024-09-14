using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionTester : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public EmotionMap map;
    void Start()
    {
        map.SetEmotion("anger");
        List<GameObject> list = new List<GameObject>();
        gameObjects.ForEach(gameObject => { 
            list.Add(Instantiate(gameObject));
        });

        map.SetObjects(list);
    }
}
