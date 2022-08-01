using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController playerController; // грубо говоря, мотор, которому нужно сказать, куда направиться
    public Transform playerCameraCoordinates;
    public float playerSpeed = 6f; // скорость игрока при ходьбе
    public float turnMovementSmoothTime = 0.1f; // параметр плавности поворота игрока при изменении направления движения
    private float turnSmoothVelocity; // переменная, необходимая для функции вычисления сглаженного угла поворота
    public Vector3 moveDirection; // направление движения


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // передвижение игрока
        moveWithWASDKeys();

        if(playerCameraCoordinates.rotation.eulerAngles.x >= 89f) // предотвратить беспорядочное вращение камеры во время приближения персонажа к её вертикали
        {
            playerController.Move(-moveDirection * (GetComponent<ThirdPersonDash>().isDashActive ? GetComponent<ThirdPersonDash>().dashSpeed + 5f : playerSpeed) * Time.deltaTime);
        }
    }

    void moveWithWASDKeys()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Raw потому что мы не хотим плавный инпут
        float vertical = Input.GetAxisRaw("Vertical"); // переменная меняет значение между -1 и 1, в зависимости от нажатой клавиши
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; // вектор движения, normalized чтобы мы по диагонали быстро не бежали
        if (direction.magnitude >= 0.1f) // если длина вектора движения больше нуля, значит должны двигаться
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCameraCoordinates.eulerAngles.y; // вычисляем угол обзора игрока
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnMovementSmoothTime); // вместо мгновенного поворота в желаемый угол, сгладим движение по rotation через метод SmoothDampAngle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // а затем используем Quaternion.Euler чтобы повернуть по оси Y игрока на сглаженный угол
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDirection * playerSpeed * Time.deltaTime); // умножаем на время, чтобы сделать движение независимым от фреймрейта
        }
    }
}
