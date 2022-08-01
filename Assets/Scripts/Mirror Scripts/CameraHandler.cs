using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform pivot;
    public Transform characterTransform;
    public Transform mTransform;

    public CharacterStatus characterStatus;
    public CameraConfig cameraConfig;
    public bool leftPivot;
    public float delta;

    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    public float smoothXVelocity;
    public float smoothYVelocity;
    public float lookAngle;
    public float titleAngle;

    private void FixedUpdate()
    {
        FixedTick();
    }

    void FixedTick()
    {
        delta = Time.deltaTime;
        HandlePosition();
        HandleRotation();
        Vector3 targetPosition = Vector3.Lerp(mTransform.position, characterTransform.position, 1);
        mTransform.position = targetPosition;
    }

    void HandlePosition()
    {
        float targetX = cameraConfig.normalX;
        float targetY = cameraConfig.normalY;
        float targetZ = cameraConfig.normalZ;

        if(characterStatus.isAiming)
        {
            targetX = cameraConfig.aimX;
            targetY = cameraConfig.aimZ;
        }

        if(leftPivot)
        {
            targetX = -targetX;
        }

        Vector3 newPivotPosition = pivot.localPosition;
        newPivotPosition.x = targetX;
        newPivotPosition.y = targetY;

        Vector3 newCameraPosition = cameraTransform.localPosition;
        newCameraPosition.z = targetZ;

        // плавно перемещаем через Lerp от оригинальных координат к новым
        float t = delta * cameraConfig.pivotSpeed;
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, newCameraPosition, 1);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newCameraPosition, 1);
    }

    void HandleRotation()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if(cameraConfig.turnSmooth > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothXVelocity, cameraConfig.turnSmooth);
        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;
        }

        lookAngle += smoothX * cameraConfig.YRotationSpeed;
        Quaternion targetRotation = Quaternion.Euler(0, lookAngle, 0);
        mTransform.rotation = targetRotation;

        titleAngle -= smoothY * cameraConfig.YRotationSpeed;
        titleAngle = Mathf.Clamp(titleAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
        pivot.localRotation = Quaternion.Euler(titleAngle, 0, 0);

    }

}
