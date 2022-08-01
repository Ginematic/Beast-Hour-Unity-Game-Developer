using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public float turnSmooth;
    public float pivotSpeed;
    public float YRotationSpeed;
    public float XRotationSpeed;
    public float minAngle;
    public float maxAngle;
    public float normalZ;
    public float normalX;
    public float aimZ;
    public float aimX;
    public float normalY;
}
