using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class AttackPlayerEnemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent Enemy;
    public bool PlayerAdvantage;
    public int health;

    float DistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, FindObjectOfType<PlayerMovement>().transform.position);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        transform.LookAt(player);
    }
    private void Update()
    {
        StartCoroutine(CatchPlayer());

        if (health <= 0)
        {
            player.GetComponent<PlayerMovement>().OnKill();
            Destroy(gameObject);
        }
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