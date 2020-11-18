using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Item : MonoBehaviour
{
    Vector3 startingPosition;
    float maxDistanceFromStart;
    float random;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        maxDistanceFromStart = .1f;
        random = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startingPosition + Vector3.up * Mathf.Sin(2*Time.realtimeSinceStartup+random) * maxDistanceFromStart;
    }
}
