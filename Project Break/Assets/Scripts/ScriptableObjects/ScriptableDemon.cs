using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Demon", menuName = "BattleSystem/New Demon")]
public class ScriptableDemon : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject Model;
    public float Charge;

    public void Recharge(float ChargeBy)
    {
        if(Charge < 100)
            Charge += ChargeBy;
    }
}
