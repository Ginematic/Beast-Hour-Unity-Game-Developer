using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    #region Singletone
    private static CustomNetworkManager _instance;
    public static CustomNetworkManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    // тут может быть ошибка, я добавил new в синглтон, чтобы избежать предупреждения
    private new void Awake()
    {
        _instance = this;
    }

    
    public GameObject lobbyCamera;
    public int victoryConditionPoints = 3;
    public int secondsUntilMatchRestarts = 3;
    public GameObject spawnPoints;
    
    public override void OnClientConnect()
    {
        //base.OnClientConnect();
        UIManager.Instance.spawnGroupToggle();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        UIManager.Instance.spawnGroupToggle();
    }

    public bool GetClientLoadedScene()
    {
        return this.clientLoadedScene;
    }

}
