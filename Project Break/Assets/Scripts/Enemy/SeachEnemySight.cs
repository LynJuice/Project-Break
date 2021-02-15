using UnityEngine;

public class SeachEnemySight : MonoBehaviour
{
    [SerializeField] SeachEnemy Parent;
    void OnTriggerEnter(Collider other)
    {
        Parent.SummonReinforcements(Random.Range(20,25),false);
    }
}
