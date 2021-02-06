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
    }
    void Update()
    {
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
    }

    void Strike()
    {
        player.GetComponent<PlayerMovement>().RecieveDamage(0.5f);
    }
}