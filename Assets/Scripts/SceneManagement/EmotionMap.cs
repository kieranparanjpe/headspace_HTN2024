using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class EmotionMap : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject rainPrefab;
    public Material angrySkybox;
    public GameObject initialFloor;
    public GameObject happyPrefab;
    public GameObject sadPrefab;
    public GameObject angryPrefab;
    private GameObject emotionPrefab;

    public void SetEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "happy":
                emotionPrefab = Instantiate(happyPrefab);
                InstantiateGrass();                
                break;

            case "anger":
                RenderSettings.skybox = angrySkybox;
                emotionPrefab = Instantiate(angryPrefab);
                break;

            case "sad":
                emotionPrefab = Instantiate(sadPrefab);
                Instantiate(rainPrefab);
                break;

            default:
                emotionPrefab = Instantiate(sadPrefab);
                Debug.LogError($"Emotion '{emotion}' not recognized!");
                break;
        }
        Destroy(initialFloor);

    }

    public void SetObjects(List<GameObject> generatedObjects)
    {

        Transform[] positions = emotionPrefab.GetComponent<IEmotionScene>().Get3DPositions();
        for (int i = 0; i < generatedObjects.Count; i++)
        {
            if(i < positions.Length)
            {
                GameObject go = generatedObjects[i];
                go.transform.SetParent(positions[i].transform, false);
            }
        }

    }

    private void InstantiateGrass()
    {
        if (grassPrefab == null)
        {
            Debug.LogError("Missing grass prefab");
            return;
        }

        for (int i = 0; i < 50; i++)
        {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(grassPrefab, randomPosition, Quaternion.identity);
        }
    }

}
