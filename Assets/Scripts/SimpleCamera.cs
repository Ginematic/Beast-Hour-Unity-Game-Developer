using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public Transform posBehindPlayer;
    public Transform playerHimself;
    public float cameraSmoothSpeed = 1f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 desiredPosition = posBehindPlayer.position + offset; // желаемая позиция = позиция точки позади игрока + желаемое смещение
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, cameraSmoothSpeed);//Vector3.Lerp(transform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime); находим позицию, которая плавно бы перемещалась к желаемой позиции каждый Update
        transform.position = smoothedPosition; // перемещаем камеру туда
        transform.LookAt(playerHimself); // фокусируемся на объекте каждый Update 
    }
}
