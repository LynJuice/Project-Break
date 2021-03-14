using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    public float seconds = 10;
    public float timer;
    public Vector3 Point;
    public Vector3 Difference;
    public Vector3 start;
    public float percent;
    void Start()
    {
        start = transform.position;
        Point = new Vector3(2400, 44, 300);
        Difference = Point - start;
    }

    void Update()
    {
        if (timer <= seconds)
        {
            timer += Time.deltaTime;
            percent = timer / seconds;
            transform.position = start + Difference * percent;
        }
    }
}
