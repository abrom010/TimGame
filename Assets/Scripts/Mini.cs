using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini : MonoBehaviour
{
    private bool enabled;
    private string name;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        name = gameObject.name;
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!enabled)
        {
            if
            (
                name=="jon" && PlayerController.jon ||
                name == "sarah" && PlayerController.sarah ||
                name == "aaron" && PlayerController.aaron ||
                name == "mars" && PlayerController.mars
            ) 
            {
                sprite.color = new Color(1f, 1f, 1f, 1f);
                enabled = true;
            }
        }
    }
}
