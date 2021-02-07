using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class AttackPlayerEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent Enemy;
    public bool PlayerAdvantage;
    float DistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, FindObjectOfType<PlayerMovement>().transform.position);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        if (PlayerAdvantage)
        { 
            
        }

        transform.LookAt(player);
    }
    private void Update()
    {
        StartCoroutine(CatchPlayer());
    }

    IEnumerator CatchPlayer()
    {
        if(PlayerAdvantage)
            yield return new WaitForSeconds(3);
        PlayerAdvantage = false;
        Enemy.destination = player.position;

        if (DistanceFromPlayer() < 2)
        {
            Enemy.isStopped = true;
            Strike();
        }
        else 
        {
            Enemy.isStopped = false;
        }
        yield return 0;
    }

    void Strike()
    {
        player.GetComponent<PlayerMovement>().RecieveDamage(0.5f);
    }
}