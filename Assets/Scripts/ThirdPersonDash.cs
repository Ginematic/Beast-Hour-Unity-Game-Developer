using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonDash : MonoBehaviour
{
    ThirdPersonMovement moveScript;

    public float dashSpeed = 20f;
    public float dashTime = 0.25f;
    public float dashCooldownTime = 4f;

    public bool isDashReady = true;
    public bool isDashActive = false;
    public bool isCharacterControlledByPlayer = true;
    public Material opponentSkinMaterial;
    public Image abilityImage1;


    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovement>();
        abilityImage1.fillAmount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isDashReady && isCharacterControlledByPlayer) // рывок
        {
            StartCoroutine(applyDash());
            abilityImage1.fillAmount = 1;
        }

        if (!isDashReady)
        {
            abilityImage1.fillAmount -= 1 / (dashCooldownTime + dashTime) * Time.deltaTime;
            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
            }
        }
    }

    private IEnumerator applyDash()
    {
        isDashReady = false;
        float startChargeTime = Time.time;
        isDashActive = true;
        while(Time.time < startChargeTime + dashTime)
        {
            moveScript.playerController.Move(moveScript.moveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashActive = false;
        yield return new WaitForSeconds(dashCooldownTime);
        isDashReady = true;
    }

        private void OnControllerColliderHit(ControllerColliderHit hitObject)
    {
        if(hitObject.gameObject.tag == "Player" && isDashActive && opponentSkinMaterial.color != Color.red)
        {
            StartCoroutine(paintDamagedPlayer());
        }
    }

    private IEnumerator paintDamagedPlayer()
    {
        opponentSkinMaterial.color = Color.red;
        yield return new WaitForSeconds(3);
        opponentSkinMaterial.color = Color.grey;
    }
}
