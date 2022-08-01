using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointMovement : MonoBehaviour
{
    public GameObject relatedObject;
    public Vector3 offsetPoint;

    private void Start()
    {
        offsetPoint = new Vector3(0, 15, -20);
    }
    void Update()
    {
        transform.position = relatedObject.transform.position + offsetPoint;
    }
}
