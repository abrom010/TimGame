using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;
    public static AnimatorController instance;
    bool hurt = false;

    private void Start()
    {
        instance = this;
    }

    public void Walk(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void Jump(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }
    public void TriggerHurt(int health)
    {
        StartCoroutine(HurtBlinker(health));
    }

    IEnumerator HurtBlinker(int health)
    {
        if(!hurt)
        {
            hurt = true;
            if (health < 1)
            {
                PlayerController.dead = true;
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().Play();

                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezePosition;

                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                sprite.color = Color.red;

                yield return new WaitForSeconds(2f);

                PlayerController.Die();

            }
            else if(!PlayerController.dead)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();

                //ignore
                int enemyLayer = LayerMask.NameToLayer("Enemy");
                int playerLayer = LayerMask.NameToLayer("Player");
                Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<EdgeCollider2D>().enabled = false;
                GetComponent<EdgeCollider2D>().enabled = true;

                //start blinking
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                sprite.color = Color.red;

                yield return new WaitForSeconds(.8f);
                if(!PlayerController.dead)
                    audio.Stop();

                //no more ignore
                Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);

                //stop blinking
                //animator.SetLayerWeight(1, 0);

                if (!PlayerController.dead)
                    sprite.color = Color.white;
            }
        }
        hurt = false;
        }

}
