using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Journal : Interactable
{
    private bool journalOpen;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        journalOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        journalOpen = !journalOpen;
        animator.SetBool("isOpen", journalOpen);

    }
}
