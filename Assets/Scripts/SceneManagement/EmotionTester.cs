using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionTester : MonoBehaviour
{
    public EmotionMap map;
    void Start()
    {
        map.SetEmotion("anger");
    }
}
