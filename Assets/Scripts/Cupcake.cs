using UnityEngine;

public class Cupcake : MonoBehaviour
{
    public float initialVelocity = 0;
    public bool isFacingLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0f, 0f, 90f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.Translate( 0f, .2f, 0f);

        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<PlayerController>())
            Destroy(gameObject);
    }
}
