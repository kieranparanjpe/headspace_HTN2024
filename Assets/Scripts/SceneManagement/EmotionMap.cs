using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class EmotionMap : MonoBehaviour
{

    public GameObject grassPrefab;

    public GameObject happyPrefab;
    public GameObject sadPrefab;
    public GameObject angryPrefab;


    public void SetEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "happy":
                Instantiate(happyPrefab);
                InstantiateGrass();                
                break;

            case "anger":
                Instantiate(angryPrefab);
                break;

            case "sad":
                Instantiate(sadPrefab);
                break;

            default:
                Debug.LogError($"Emotion '{emotion}' not recognized!");
                break;
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
