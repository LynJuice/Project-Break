using System.Collections;
using UnityEngine;

[System.Serializable]
public class BaseEnemy
{
    public string Name;
    public enum Type{
        Ice,
        Fire,
        Electric,
        Wind,
        Steel,
        Blessed,
        Cursed,
    }
    public enum Rarity
    { 
        Common,
        Uncommmon,
        Rare,
        Special // To be removed?
    }

    public Type EnemyType;
    public Rarity rarity;

    [Header("Health")]
    public float BaseHp;
    public float CurHp;

    [Header("Charge")]
    public float BaseMp;
    public float CurMp;

    [Header("Attack & Defend")]
    public int baseATK;
    public int curATK;
    public int BaseDef;
    public int curDef;
}
