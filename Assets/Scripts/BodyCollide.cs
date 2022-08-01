using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollide : MonoBehaviour
{
    public Collider dasherCollider;

    private void Start()
    {
        dasherCollider = GetComponent<Collider>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Controller");
    }
}
