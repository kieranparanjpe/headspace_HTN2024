using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Journal Opened.");
            //play animation, stop at frame 60.

        }
    }
}
