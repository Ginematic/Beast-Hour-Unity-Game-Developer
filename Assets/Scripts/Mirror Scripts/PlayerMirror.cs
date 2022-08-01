using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMirror : NetworkBehaviour
{
    // SyncVar - переменная будет на сервере + постоянно синхронизироваться с клиентом, чтобы избежать жульничества
    // SerializeField - чтобы закрытое поле было видно в инспекторе

    [Header("Movement")]
    [SyncVar][SerializeField] private float playerSpeed; // скорость игрока при ходьбе
    public CharacterController playerController; // грубо говоря, мотор, которому нужно сказать, куда направиться
    public float turnMovementSmoothTime = 0.1f; // параметр плавности поворота игрока при изменении направления движения
    private float turnSmoothVelocity; // переменная, необходимая для функции вычисления сглаженного угла поворота
    private Vector3 moveDirection; // направление движения
    public GameObject playerCamera;

    [Header("Dash")]
    [SyncVar][SerializeField] private float invulnerabilityTimeAfterTakingDamage;
    [SyncVar] private float dashSpeed;  //20 переменная скорости в первую очередь зависит от требуемого расстояния
    [SyncVar] public float dashTime = 0.25f; // время рывка зафиксировано
    [SyncVar] public float dashDistance = 5; // именно эта переменная должна отображаться в инспекторе, быть изменяемой и влиять на скорость рывка
    [SyncVar] public float dashCooldownTime = 4f;
    public bool isDashReady = true;
    private bool isDashActive = false;

    //public Material opponentSkinMaterial;
    public MeshRenderer playerHeadMaterial;
    public Image abilityImage1;


    [Header("Stats")]
    [SyncVar] public string playerName;
    [SyncVar(hook="MyHook")] public int fragPoints = 0;
    public TextMesh scoreBar;

    private void Start() // делаем на старте а не на awake
    {

        if (isClient && isLocalPlayer) // если клиент
        {
            InputManagerScript.Instance.SetPlayer(this); // обращаемся к синглтону и даем ему ссылку на скрипт игрока
            UIManager.Instance.playerTagContainer = playerCamera.transform.GetChild(0).GetChild(2).gameObject;
            UIManager.Instance.playerTagField = playerCamera.transform.GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
            //playerCamera = this.transform.GetChild(1).gameObject;
            playerCamera.SetActive(true);
            abilityImage1.fillAmount = 0;
        }
        if (isServer) // если сервер
        {
            playerSpeed = 6f; // выставляем единую скорость
            invulnerabilityTimeAfterTakingDamage = 3f;
            CmdSetPlayerName(PlayerManager.Instance.getPlayerNameBuffer());
        }
    }

    private void Update()
    {
        scoreBar.text = new string('-', fragPoints);
    }

    public void MovePlayer(Vector3 movVect) // косметическая функция для клиента, идентичная той, что на сервере. Результаты её работы не будут влиять на игру на самом деле, она нужна для плавности и отзывчивости на клиенте
    {
        if (movVect.magnitude >= 0.1f) // если длина вектора движения больше нуля, значит должны двигаться
        {
            float targetAngle = Mathf.Atan2(movVect.x, movVect.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y; // вычисляем угол обзора игрока
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnMovementSmoothTime); // вместо мгновенного поворота в желаемый угол, сгладим движение по rotation через метод SmoothDampAngle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // а затем используем Quaternion.Euler чтобы повернуть по оси Y игрока на сглаженный угол
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDirection * playerSpeed * Time.deltaTime); // умножаем на время, чтобы сделать движение независимым от фреймрейта
        }
    }

    public void RotatePlayerWithMouse(Vector2 axis)
    {
        transform.localRotation = Quaternion.Euler(0, axis.x, 0); // вращаем игрока и камеру по оси Х
    }

    public IEnumerator applyDash()
    {
        isDashReady = false;
        float startChargeTime = Time.time;
        isDashActive = true;
        while (Time.time < startChargeTime + dashTime)
        {
            playerController.Move(moveDirection * (dashDistance / dashTime) * Time.deltaTime);
            yield return null;
        }
        isDashActive = false;
        yield return new WaitForSeconds(dashCooldownTime);
        isDashReady = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hitObject)
    {
        Debug.Log("OnControllerColliderHit");
        if (hitObject.gameObject.tag == "Player" && isDashActive)
        {
            Debug.Log("if (hitObject.gameObject.tag == Player && isDashActive)");
            if (hitObject.gameObject.GetComponent<PlayerMirror>().playerHeadMaterial.sharedMaterial != PlayerManager.Instance.damagedHeadMaterial)
            {
                Debug.Log("++fragPoints;");
                ++fragPoints;
                StartCoroutine(paintDamagedPlayer(hitObject.gameObject.GetComponent<PlayerMirror>().playerHeadMaterial));
                if (fragPoints >= CustomNetworkManager.Instance.victoryConditionPoints) // считаем количество победных очков
                {
                    Debug.Log("StartCoroutine victoryAndGameReset");
                    StartCoroutine(victoryAndGameReset(playerName));
                    
                    //StartCoroutine(CustomNetworkManager.Instance.victoryAndGameReset());
                }
            }
        }
    }

    private void MyHook(int oldValue, int newValue)
    {
        Debug.Log("Hook Called: OldValue=" + oldValue + "  NewValue=" + newValue);
        fragPoints = newValue;  //I understand why this shouldn't set dirty bit, because it would cause infinite looping
    }

    private IEnumerator paintDamagedPlayer(MeshRenderer mesh)
    {
        mesh.sharedMaterial = PlayerManager.Instance.damagedHeadMaterial;
        yield return new WaitForSeconds(invulnerabilityTimeAfterTakingDamage);
        mesh.sharedMaterial = PlayerManager.Instance.fullHPHeadMaterial;
    }

    [Command]
    public void CmdSetPlayerName(string name)
    {
        playerName = name;
    }

    [ClientRpc]
    public void RpcTagToggle(string name)
    {
        Debug.Log("Victory of <" + name + "> player!");
        UIManager.Instance.playerTagField.text = name;
        UIManager.Instance.playerTagToggle();
        fragPoints = 0;
    }

    
    private IEnumerator victoryAndGameReset(string name)
    {
        
        RpcTagToggle(name);
        yield return new WaitForSeconds(CustomNetworkManager.Instance.secondsUntilMatchRestarts);
        RpcTagToggle("");
        int rand = Random.Range(0, 3);
        this.transform.Translate(CustomNetworkManager.Instance.spawnPoints.transform.GetChild(rand).transform.position);
    }

}