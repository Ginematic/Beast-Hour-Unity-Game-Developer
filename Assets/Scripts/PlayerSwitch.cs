using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public GameObject otherPlayer;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            GetComponent<ThirdPersonMovement>().enabled = false;
            GetComponent<ThirdPersonDash>().isCharacterControlledByPlayer = false;
            //GetComponent<ThirdPersonDash>().enabled = false;
            GetComponent<ThirdPersonMovement>().playerCameraCoordinates.gameObject.SetActive(false);

            otherPlayer.GetComponent<ThirdPersonMovement>().enabled = true;
            otherPlayer.GetComponent<ThirdPersonDash>().isCharacterControlledByPlayer = true;
            otherPlayer.GetComponent<ThirdPersonMovement>().playerCameraCoordinates.gameObject.SetActive(true);
            otherPlayer.GetComponent<PlayerSwitch>().enabled = true;

            GetComponent<PlayerSwitch>().enabled = false;
        }
    }
}
