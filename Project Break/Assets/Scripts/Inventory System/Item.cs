using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "InventorySystem/New Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Discription;

    public bool StoryItem;

    public bool Sellable;
    public int SellCost;
    public int BuyCost;

    public int ChargeIncrese;
    public int HealthIncrese;
}
