using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
  // [SerializeField] float MaxAngle, MinAngle;
    [SerializeField] Transform LookAt;

    void Update()
    {
        transform.LookAt(LookAt, Vector3.up);

        /*
        float X = Mathf.Clamp(transform.position.x, MinAngle, MaxAngle);
        transform.rotation = Quaternion.Euler(X,Quaternion.identity.y, Quaternion.identity.z);
      *///Broken
    }
}
