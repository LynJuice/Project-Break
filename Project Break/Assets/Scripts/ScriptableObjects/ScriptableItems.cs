using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "BattleSystem/New Item")]
public class ScriptableItems : ScriptableObject
{
    public string Name;
    public int HealBy;
    public int ChargeBy;
}
