using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class SeachEnemy : MonoBehaviour
{
    [SerializeField] Transform[] ExtrasspawnPos;
    [SerializeField] AttackPlayerEnemy[] Spawn;
    [SerializeField] NavMeshAgent NMA;
    [SerializeField] Transform[] points;
    int Health = 100;
    int destPoint = 0;
    [SerializeField] float TimeToWait;
    bool Reached;
    public void GetDamage(int Damage)
    {
        Health -= Damage;
    }

    public bool IsDead()
    {
        if (Health <= 0)
            return true;

        Debug.LogError("Couldn't Determine Death");
        return false;
    }

    void Start()
    {
        if (!NMA.pathPending && NMA.remainingDistance < 0.5f)
            StartCoroutine(GotoNextPoint());
    }

    IEnumerator GotoNextPoint()
    {
        if (points.Length == 0)
            Debug.LogError("No assigned points for " + gameObject.name);

        yield return new WaitForSeconds(TimeToWait);
        NMA.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
        Reached = true;
    }

    void Update()
    {
        if (!NMA.pathPending && NMA.remainingDistance < 0.5f && Reached)
        {
            Reached = false;
            StartCoroutine(GotoNextPoint());
        }
    }
    
    public void SummonReinforcements(int Many)
    {
        for (int i = 0; i < Many; i++)
        {
            Instantiate(Spawn[Random.Range(0, Spawn.Length)], ExtrasspawnPos[i].position, Quaternion.identity);
        }
    }
}
