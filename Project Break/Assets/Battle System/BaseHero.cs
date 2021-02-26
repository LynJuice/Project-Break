using System.Collections;
using UnityEngine;

[System.Serializable]
public class BaseHero
{
    public string Name;

    [Header("Health")]
    public float BaseHp;
    public float CurHp;

    [Header("Charge")]
    public float BaseMp;
    public float CurMp;

    [Header("Stats")]
    public int Stamina;
    public int intellect;
    public int agility;
    public int dexterity;
}
