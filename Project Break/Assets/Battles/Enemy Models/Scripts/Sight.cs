using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] SearchingEnemy Parent;
    void OnTriggerEnter(Collider other)
    {
        Parent.PlayerSpotted = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Parent.PlayerSpotted = false;
    }
}
