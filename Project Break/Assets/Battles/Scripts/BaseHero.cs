using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseHero
{
    public string Name;

    [Space]

    public Rune rune;

    [Header("Icons")]
    public Sprite Icon;
    public Sprite IconDown;

    [Header("Health")]
    public float BaseHp;
    public float CurHp;

    [Header("Spirit")]
    public BaseSpirt Spirit;
    public float BaseMp;
    public float CurMp;

    [Header("Defend & Attack")]
    public int CurDef;
    public int BaseDef;
    public int CutAtk;
    public int BaseAtk;

    [Header("Other")]
    public Transform CameraPos;
}

[System.Serializable]
public class BaseSpirt
{
    public string SpiritsName;
    public Eliment Weakness;
    public GameObject Model;

    public List<Power> Powers = new List<Power>(6); // MAX 6

    public enum Eliment
    {
        Ice,
        Fire,
        Electric,
        Wind,
        Blessed,
        Cursed,
    }
}
