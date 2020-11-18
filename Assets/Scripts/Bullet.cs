using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initialVelocity = 0;
    public bool isFacingLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFacingLeft)
            transform.Translate(-(Time.deltaTime * (10+Mathf.Abs(initialVelocity)*.1f)), 0f, 0f);
        else
            transform.Translate(Time.deltaTime * (10+initialVelocity*.1f), 0f, 0f);

        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            Destroy(gameObject);
    }
}
