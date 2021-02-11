using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseKatana : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
      //  if (other.tag == "Enemy")
        {
            Debug.Log(other.name);
            other.GetComponent<AttackPlayerEnemy>().health -= 100; //Random.Range(25,50);
        }
    }
}
