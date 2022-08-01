using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //public Vector3 direction;
    private Rigidbody playerPhysics;
    public float playerSpeed = 10f;
    //public TextMeshProUGUI collectiblesText;
    //private uint collectiblesAmount = 0;
    

    private void Awake()
    {
        playerPhysics = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) // СЂРµСЃС‚Р°СЂС‚ РїРѕР·РёС†РёРё СЃС„РµСЂС‹
        {
            playerPhysics.isKinematic = true;
            transform.position = new Vector3(0, 2, 0);
            playerPhysics.isKinematic = false;
            Debug.Log("Position has been reset.");
        }
        if (Input.GetKeyDown(KeyCode.Space)) // РѕСЃС‚Р°РЅРѕРІРёС‚СЊ РїСЂРµРґС‹РґСѓС‰РёР№ РёРјРїСѓР»СЊСЃ Рё РїСЂРёРґР°С‚СЊ РЅРѕРІС‹Р№ РІРІРµСЂС… РЅР° Space
        {
            playerPhysics.isKinematic = true;
            playerPhysics.isKinematic = false;
            playerPhysics.AddForce(Vector3.up * playerSpeed, ForceMode.Impulse);
            Debug.Log("Jump.");

        }
        if (Input.GetKeyDown(KeyCode.W)) // РёРјРїСѓР»СЊСЃ РІРїРµСЂРµРґ Рё С‡СѓС‚СЊ РІРІРµСЂС…
        {
            playerPhysics.isKinematic = true;
            playerPhysics.isKinematic = false;
            playerPhysics.AddForce(new Vector3(1, 0.3f, 0) * playerSpeed, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.S)) // РёРјРїСѓР»СЊСЃ РЅР°Р·Р°Рґ Рё С‡СѓС‚СЊ РІРІРµСЂС…
        {
            playerPhysics.isKinematic = true;
            playerPhysics.isKinematic = false;
            playerPhysics.AddForce(new Vector3(-1, 0.3f, 0) * playerSpeed, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.A)) // РёРјРїСѓР»СЊСЃ РІР»РµРІРѕ Рё С‡СѓС‚СЊ РІРІРµСЂС…
        {
            playerPhysics.isKinematic = true;
            playerPhysics.isKinematic = false;
            playerPhysics.AddForce(new Vector3(0, 0.3f, 1) * playerSpeed, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.D)) // РёРјРїСѓР»СЊСЃ РІР»РµРІРѕ Рё С‡СѓС‚СЊ РІРІРµСЂС…
        {
            playerPhysics.isKinematic = true;
            playerPhysics.isKinematic = false;
            playerPhysics.AddForce(new Vector3(0, 0.3f, -1) * playerSpeed, ForceMode.Impulse);
        }
    }
}
;