using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class EmotionMap : MonoBehaviour
{
    public GameObject rainPrefab;
    [SerializeField] private Material angrySkybox;
    [SerializeField]  private Material sadSkybox;

    public GameObject initialFloor;
    public GameObject happyPrefab;
    public GameObject sadPrefab;
    public GameObject angryPrefab;
    public GameObject ambiantNoice;
    private GameObject emotionPrefab;

    public void SetEmotion(string emotion)
    {
        ambiantNoice.SetActive(false);
        switch (emotion.ToLower())
        {
            case "happy":
            case "happiness":
                this.emotionPrefab = Instantiate(happyPrefab);
                break;

            case "anger":
            case "angry":
                RenderSettings.skybox = angrySkybox;
                this.emotionPrefab = Instantiate(angryPrefab);
                break;

            case "sad":
            case "sadness":
                RenderSettings.skybox = sadSkybox;
                this.emotionPrefab = Instantiate(sadPrefab);
                //Instantiate(rainPrefab);
                break;

            default:
                this.emotionPrefab = Instantiate(sadPrefab);
                Debug.LogError($"Emotion '{emotion}' not recognized!");
                break;
        }
        Destroy(initialFloor);

    }

    public void SetObjects(List<GameObject> generatedObjects)
    {

        Transform[] positions = this.emotionPrefab.GetComponent<IEmotionScene>().Get3DPositions();
        for (int i = 0; i < generatedObjects.Count; i++)
        {
            if(i < positions.Length)
            {
                GameObject go = generatedObjects[i];
                go.transform.SetParent(positions[i].transform, false);
            }
        }

    }

    public void SetObject(GameObject obj, int i){
        Transform[] positions = emotionPrefab.GetComponent<IEmotionScene>().Get3DPositions();

        Debug.Log(i);
        i %= positions.Length;
        if(i < positions.Length)
        {
            obj.transform.SetParent(positions[i].transform, false);
            obj.transform.position = positions[i].transform.position;
            
            // F U N F A C T O R
            GameObject objCopy = Instantiate(obj, obj.transform.position + new Vector3(0,200,0), obj.transform.rotation);
            objCopy.AddComponent<Rigidbody>();
            objCopy.AddComponent<BoxCollider>();
            
            Debug.Log(positions[i].transform.position);
        }
        
    }

    

}
