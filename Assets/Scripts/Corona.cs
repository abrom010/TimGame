using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corona : MonoBehaviour
{
    Vector3 startingPosition;
    float maxDistanceFromStart;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        maxDistanceFromStart = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startingPosition + Vector3.up * Mathf.Sin(Time.realtimeSinceStartup) * maxDistanceFromStart;
    }
}
