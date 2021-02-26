using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SearchingEnemy : MonoBehaviour
{
    [Header("Catch Player")]
    public bool PlayerSpotted;
    public bool EnemyInRange;
    public bool RayhitPlayer;
    bool seen;
    [Space]
    Transform Player;
    [SerializeField] float PlayerSeekRadius;
    [SerializeField] float PlayerReachRadius;
    [SerializeField] LayerMask EverythingButPlayer;

    [Header("Wander Around")]
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;
    NavMeshAgent agent;
    float timer;
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        Player = FindObjectOfType<PlayerMovement>().transform;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) < 1.5f)
            Player.GetComponent<PlayerMovement>().startBattle(1);

        timer += Time.deltaTime;
        Vector3 dir = Player.position - transform.position;

        if (Vector3.Distance(transform.position, Player.position) < PlayerSeekRadius)
            EnemyInRange = true;
        else
            EnemyInRange = false;

        RayhitPlayer = Physics.Raycast(transform.position, dir, Mathf.Infinity, EverythingButPlayer);

        if(PlayerSpotted && RayhitPlayer || seen && EnemyInRange)
        {
            seen = true;
            agent.SetDestination(Player.position);
        }
        else
        {
            if(seen)
                StartCoroutine(Cooldown());

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(25);
        seen = false;
    }
}
