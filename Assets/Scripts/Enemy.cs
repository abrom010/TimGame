using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health;

    void Start()
    {
        if (name == "Cake")
        {
            health = 50;
        }
        else
        {
            health = 4;
        }
    }


    void Hurt()
    {
        if (--health < 0) Destroy(gameObject);
        else StartCoroutine(HurtBlinker());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        if (bullet != null)
        {
            Hurt();
        }
    }

    IEnumerator HurtBlinker()
    {

        //start blinking
        //animator.SetLayerWeight(1, 1);
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.red;

        //wait
        yield return new WaitForSeconds(.2f);

        //stop blinking
        //animator.SetLayerWeight(1, 0);
        sprite.color = Color.white;
    }
}
