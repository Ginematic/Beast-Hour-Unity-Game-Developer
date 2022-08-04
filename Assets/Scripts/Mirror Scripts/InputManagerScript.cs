using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InputManagerScript : MonoBehaviour
{
    #region Singletone
    private static InputManagerScript _instance;
    public static InputManagerScript Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;
    }

    private Vector3 movementVector;
    private Vector2 mouseAxisInput;
    public float mouseSensitivity = 1.5f;
    //public GameObject cameraForMovementDirection;

    [SerializeField]
    private PlayerMirror playerObj; // объект игрока


    private void Update() // отслеживаем все нажатия игрока
    {
        if(playerObj && NetworkClient.active) // если игровой объект игрока существует и клиент активен
        {
            MoveInput();
            MouseLookInput();
            if (Input.GetMouseButtonDown(0) && playerObj.isDashReady)
            {
                StartCoroutine(playerObj.CmdApplyDash());
                playerObj.abilityImage1.fillAmount = 1;
            }
            if (!playerObj.isDashReady)
            {
                playerObj.abilityImage1.fillAmount -= 1 / (playerObj.dashCooldownTime + playerObj.dashTime) * Time.deltaTime;
                if (playerObj.abilityImage1.fillAmount <= 0)
                {
                    playerObj.abilityImage1.fillAmount = 0;
                }
            }
        }
    }

    public void SetPlayer(PlayerMirror pl)
    {
        playerObj = pl;
    }

    // public void SpawnPlayer()
    // {
    //     PlayerManager.Instance.SpawnPlayer();
    // }

    private void MoveInput() // информация о вводе игрока передается в качестве вектора функциям передвижения
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Raw потому что мы не хотим плавный инпут
        float vertical = Input.GetAxisRaw("Vertical"); // переменная меняет значение между -1 и 1, в зависимости от нажатой клавиши
        movementVector = new Vector3(horizontal, 0f, vertical).normalized; // вектор движения, normalized чтобы мы по диагонали быстро не бежали
        playerObj.MovePlayer(movementVector);
    }

    private void MouseLookInput()
    {
        mouseAxisInput.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        playerObj.RotatePlayerWithMouse(mouseAxisInput);
        //playerObj.
    }
}
