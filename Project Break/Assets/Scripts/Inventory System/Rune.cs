using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "InventorySystem/New Rune")]
public class Rune : ScriptableObject
{
    public enum Rarity
    { 
        Common,
        Uncommon,
        Rare,
        Special
    };
    public Rarity rarity;
    public string Name;
    public string Discription;

    [Header("Rune Power")]
    public int Defence;
    public int Attack;
    public int Health;
    public int MP;

    public Power SpecialPower;
}
