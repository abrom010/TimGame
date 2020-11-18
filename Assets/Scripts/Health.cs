using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = PlayerController.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.health > 3) health = 3;
        else health = PlayerController.health;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

        }
    }
}
