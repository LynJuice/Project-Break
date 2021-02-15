using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class SeachEnemy : MonoBehaviour
{
    public GameObject ToBeAttackedShow;
    [SerializeField] PlayerMovement PM;
    [SerializeField] Transform[] ExtrasspawnPos;
    [SerializeField] AttackPlayerEnemy[] Spawn;
    [SerializeField] NavMeshAgent NMA;
    [SerializeField] Transform[] points;
    int destPoint = 0;
    [SerializeField] float TimeToWait;
    bool Reached;
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
        if (Vector3.Distance(PM.transform.position, transform.position) < 10)
            ToBeAttackedShow.SetActive(true);
        else
            ToBeAttackedShow.SetActive(false);

        if (!NMA.pathPending && NMA.remainingDistance < 0.5f && Reached)
        {
            Reached = false;
            StartCoroutine(GotoNextPoint());
        }
    }
    
    public void SummonReinforcements(int Many,bool Advantige)
    {
        for (int i = 0; i < Many; i++)
        {
            if (i < ExtrasspawnPos.Length)
            {
                AttackPlayerEnemy APE = Instantiate(Spawn[Random.Range(0, Spawn.Length)], ExtrasspawnPos[i].position, Quaternion.identity);
                APE.PlayerAdvantage = Advantige;
            }
            else Debug.LogWarning("More Spawnning Places Needed");
        }
        PM.SetPlayerState(true);
        Destroy(gameObject);
    }
}
