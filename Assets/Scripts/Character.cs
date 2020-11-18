using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        string name = gameObject.name;
        if (name=="jon")
            PlayerController.jon = true;
        else if (name=="sarah")
            PlayerController.sarah = true;
        else if (name=="aaron")
            PlayerController.aaron = true;
        else if (name=="mars")
            PlayerController.mars = true;

        //SpriteRenderer[] spriteArray = collision.gameObject.GetComponentsInChildren<SpriteRenderer>();
        //foreach (SpriteRenderer sprite in spriteArray)
        //{
        //    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        //}
    }
}
