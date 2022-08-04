using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : MonoBehaviour
{
    #region Singletone
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public CustomNetworkManager netManager;
    public Material fullHPHeadMaterial;
    public Material damagedHeadMaterial;


    [SerializeField]
    private string playerNameBuffer;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnPlayer() // при нажатии на кнопку спавна
    {
        playerNameBuffer = UIManager.Instance.submitInputField();
        if (playerNameBuffer == "")
        {
            Debug.LogError("No name for player!");
            return;
        }

        

        if (!netManager.GetClientLoadedScene())
        {
            //Debug.Log("Client Loaded Scene");
            if (!NetworkClient.ready)
                //Debug.Log("Trying to send ready");
                NetworkClient.Ready();

            if (netManager.autoCreatePlayer)
                //Debug.Log("Adding player");
                NetworkClient.AddPlayer();

            // подготовка интерфейса к игре
            UIManager.Instance.spawnGroupToggle();
            UIManager.Instance.playerTagToggle();
            UIManager.Instance.submitPlayerTag(playerNameBuffer);
            Cursor.lockState = CursorLockMode.Locked; // отключаем видимость мыши

            CustomNetworkManager.Instance.lobbyCamera.SetActive(false);
        }
    }

    public string getPlayerNameBuffer()
    {
        return playerNameBuffer;
    }
}
