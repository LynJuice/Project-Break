using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    public string Attacker;
    public string Type;

    public GameObject AttackerGameObject;
    public GameObject AttackersTarget;

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
