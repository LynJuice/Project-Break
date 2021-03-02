using System.Collections;
using UnityEngine;

[System.Serializable]
public class BaseHero
{
    public string Name;

    [Space]

    [Header("Icons")]
    public Sprite Icon;
    public Sprite IconDown;

    [Header("Health")]
    public float BaseHp;
    public float CurHp;

    [Header("Charge")]
    public float BaseMp;
    public float CurMp;

    [Header("Defend & Attack")]
    public int CurDef;
    public int BaseDef;
    public int CutAtk;
    public int BaseAtk;

    [Header("Stats")]
    public int Stamina;
    public int intellect;
    public int agility;
    public int dexterity;
}
