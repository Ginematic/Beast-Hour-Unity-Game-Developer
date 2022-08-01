using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawners = new GameObject[8]; 
    public GameObject[] player = new GameObject[2]; 
    public GameObject[] playerCamera = new GameObject[2];
    public GameObject[] playerPoints = new GameObject[2];
    public GameObject playerPrefab;
    public Material[] headMaterial = new Material[2]; 

    List<int> respawnNumbers = new List<int>(){0, 1, 2, 3, 4, 5, 6, 7}; 
    ThirdPersonDash dashScript; 

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject; 
        }
    }

    private void Start()
    {
        int rand; 
        for (int i = 0; i < 2; i++)
        {
            rand = Random.Range(0, respawnNumbers.Count - 1); 
            player[i] = Instantiate(playerPrefab, spawners[rand].transform.position, spawners[rand].transform.rotation); 
            player[i].name = "Player " + (i + 1);
            respawnNumbers.Remove(rand); 
            GameObject playerHead = player[i].transform.Find("Body").gameObject.transform.Find("Head").gameObject;
            playerHead.GetComponent<Renderer>().sharedMaterial = headMaterial[i]; // добавление материала головы
            player[i].GetComponent<ThirdPersonDash>().opponentSkinMaterial = headMaterial[1 - i]; // хранение материала головы оппонента

            player[i].GetComponent<ThirdPersonMovement>().playerCameraCoordinates = playerCamera[i].transform; // привязка игрока к камере
            playerCamera[i].GetComponent<SimpleCamera>().playerHimself = player[i].transform; // привязка камеры к игроку
            playerPoints[i].GetComponent<CameraPointMovement>().relatedObject = player[i];
            playerPoints[i].GetComponent<CameraPointMovement>().enabled = true;
            playerCamera[i].transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Player " + (i + 1); // вывод на интерфейс камеры никнейм игрока
            player[i].GetComponent<ThirdPersonDash>().abilityImage1 = playerCamera[i].transform.GetChild(0).transform.GetChild(2).GetComponent<Image>(); // привязка игрока к изображению способности
        }

        
        player[1].GetComponent<ThirdPersonMovement>().enabled = false;
        //player[1].GetComponent<ThirdPersonDash>().enabled = false;
        player[1].GetComponent<ThirdPersonDash>().isCharacterControlledByPlayer = false;
        player[1].GetComponent<PlayerSwitch>().enabled = false;
        
        // привязываем камеры
        
        playerCamera[0].SetActive(true);

        // ссылки для переключения персонажей
        player[0].GetComponent<PlayerSwitch>().otherPlayer = player[1];
        player[1].GetComponent<PlayerSwitch>().otherPlayer = player[0];
    }
}
