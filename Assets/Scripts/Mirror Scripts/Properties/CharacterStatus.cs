using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/status")]
public class CharacterStatus : ScriptableObject
{
    public bool isAiming;
    public bool isSprinting;
    public bool isGrounding;
}
