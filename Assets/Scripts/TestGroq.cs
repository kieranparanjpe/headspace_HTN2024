using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGroq : MonoBehaviour
{
    private const string envFile = "Assets/local.env";

    void Start()
    {
        
        DotEnvLoader.LoadEnvFile(envFile);

        GroqService groqService = new GroqService(this);
        
        groqService.GetResponse("Hi how are you doing?", callback);

    }

    public void callback(string s) {

    }

}
