using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Cake : MonoBehaviour
{
    private bool awake;
    public GameObject cupcake;
    float startTime;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        awake = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time-startTime>1f && awake)
        {
            startTime = Time.time;
            GameObject go = Instantiate(cupcake, new Vector3(transform.position.x - 2.5f, transform.position.y-.5f, transform.position.z), Quaternion.identity) as GameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!awake)
        {
            animator.SetBool("isAwake", true);
            awake = true;
            startTime = Time.time;
        }
            
    }
}
