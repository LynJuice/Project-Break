using UnityEngine;

public class SwordCollisions : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<SeachEnemy>().GetDamage(Random.Range(25,75));
            
        }
    }
}
