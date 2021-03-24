using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Shop shop;
    void OnTriggerEnter(Collider other)
    {
        shop.gameObject.SetActive(true);
    }
}
