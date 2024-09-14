using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EmotionMap : MonoBehaviour
{
    private IEmotionScene currentEmotionScene;

    public void SetEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "happy":
                currentEmotionScene = new Happy(); 
                break;

            case "anger":
                currentEmotionScene = new Anger();
                break;

            case "sad":
                currentEmotionScene = new Sad();
                break;

            default:
                Debug.LogError($"Emotion '{emotion}' not recognized!");
                break;
        }
    }

    public IEmotionScene GetCurrentEmotionScene()
    {
        return currentEmotionScene;
    }
}
