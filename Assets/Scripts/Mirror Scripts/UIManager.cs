using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singletone
    private static UIManager _instance;
    public static UIManager Instance
    {
    get
    {
        return _instance;
    }
    }
    #endregion

    [SerializeField]
    public GameObject spawnGroupContainer; // контейнер интерфейса перед спавном игрока

    [SerializeField]
    public GameObject playerTagContainer; // контейнер тега игрока


    public InputField nicknameInputField; // поле для ввода имени игрока
    public Text playerTagField; // поле текста тега

    private void Awake()
    {
        _instance = this;
    }

    public void spawnGroupToggle() // переключение поля ввода никнейма и кнопки спавна
    {
        spawnGroupContainer.SetActive(!spawnGroupContainer.activeSelf);
    }

    public string submitInputField()
    {
        return nicknameInputField.text;
    }

    public void playerTagToggle() // переключение видимости тега игрока
    {
        playerTagContainer.SetActive(!playerTagContainer.activeSelf);
    }

    public void submitPlayerTag(string name)
    {
        playerTagField.text = name;
    }

}
