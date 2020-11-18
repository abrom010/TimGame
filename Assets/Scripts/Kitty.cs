using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitty : MonoBehaviour
{
    private bool awake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!awake)
        {
            if(GetComponent<Renderer>().isVisible)
            {
                GetComponent<Rigidbody2D>().WakeUp();
            }
        }
    }
}
