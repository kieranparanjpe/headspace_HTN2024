using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float rayDistance;
    [SerializeField] private Journal journal;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    // Start is called before the first frame update
    private void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }
    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        RaycastHit hitInfo;


        if(Physics.Raycast(ray, out hitInfo, rayDistance, mask))
        {
            Interactable i = hitInfo.collider.GetComponent<Interactable>();
            if (i != null && !(journal.isOpen()))
            {
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    i.BaseInteract();
                }
            }
        }
    }
}
