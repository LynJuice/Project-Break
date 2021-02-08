using UnityEngine;
[CreateAssetMenu(fileName = "Demon", menuName = "BattleSystem/New Demon")]
public class ScriptableDemon : ScriptableObject
{
    public GameObject Model;
    public float Charge;

    public void Recharge(float ChargeBy)
    {
        if(Charge < 100)
            Charge += ChargeBy;
    }
}
