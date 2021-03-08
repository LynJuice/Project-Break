using UnityEngine;
[CreateAssetMenu(fileName = "Power", menuName = "BattleSystem/New Power")]
public class Power : ScriptableObject
{
    public enum ApplyEliment
    {
        Ice,
        Fire,
        Electric,
        Wind,
        Blessed,
        Cursed,
        Other,
        None
    }
    public ApplyEliment ElimentToApply;

    public string Name;
    
    public int ChargeCost;
    public int HealthCost;

    public int BaseDam;
    public int MaxDam;

    public int BaseHeal;
    public int MaxHeal;
}
