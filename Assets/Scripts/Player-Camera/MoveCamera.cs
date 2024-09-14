using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private float cameraHeight;

    private Vector3 yOffset;

    private void Start()
    {
        yOffset = new Vector3(0, cameraHeight, 0);
    }
    private void Update()
    {
        transform.position = cameraPosition.position + yOffset;
    }
}
