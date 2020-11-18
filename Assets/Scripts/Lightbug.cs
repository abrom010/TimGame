using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lightbug : MonoBehaviour
{
    Vector3 startingPosition;
    Vector3 randomPosition;
    float startTime, totalDistance, maxDistanceFromStart;
    
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startingPosition = transform.position;
        randomPosition = startingPosition + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        maxDistanceFromStart = 1;
        Lerp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        totalDistance = Vector3.Distance(startingPosition, randomPosition);
        float currentDuration = (Time.time - startTime)*.5f;
        float lerp = currentDuration / totalDistance;

        transform.position = Vector3.Lerp(startingPosition,randomPosition,lerp);
        transform.position += .25f*(Vector3.up * Mathf.Sin(12 * Time.realtimeSinceStartup) * maxDistanceFromStart);
        transform.position += .5f*(Vector3.left * Mathf.Sin(6 * Time.realtimeSinceStartup) * maxDistanceFromStart);
    }

    public IEnumerator Lerp()
    {
            Debug.Log("lerping");
            startTime = Time.time;
            randomPosition = GetRandomPositionWithinBounds();
            startingPosition = transform.position;
        
        yield return new WaitForSeconds(4f);
    }

    Vector3 GetRandomPositionWithinBounds()
    {
        return new Vector3(Random.Range(-13f,1f), Random.Range(-6f, 0f), 0);
    }

}
