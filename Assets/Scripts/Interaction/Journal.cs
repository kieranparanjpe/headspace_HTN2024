using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class Journal : Interactable
{
    private bool journalOpen;
    private bool canCloseJournal;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject pageText;
    public Picovoice pv;
    public TextMeshProUGUI rightText = null;

    // Start is called before the first frame update
    void Start()
    {
        journalOpen = false;
        canCloseJournal = false;
        pageText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (journalOpen && canCloseJournal)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }else if (Input.GetKeyDown(KeyCode.G))
            {
                Interact();
            }

            rightText.text = pv.Transcript;
        }
    }

    protected override void Interact()
    {
        Debug.Log("hi");
        journalOpen = !journalOpen;
        if (journalOpen)
            pv.OpenJournal();
        else
            pv.CloseJournal();
        Debug.Log(journalOpen);
        animator.SetBool("isOpen", journalOpen);
        StartCoroutine(FadeText());

    }
    
    IEnumerator FadeText()
    {

        if (journalOpen)
        {
            yield return new WaitForSeconds(2f);
            pageText.SetActive(true);
            canCloseJournal = true;
        }
        else
        {
            yield return new WaitForSeconds(0f);
            pageText.SetActive(false);
            canCloseJournal = false;
        }


    }

    public Boolean isOpen()
    {
        return journalOpen;
    }


}
