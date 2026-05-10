using UnityEngine;

public class IgnoreCollisions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int k = i + 1; k < colliders.Length; k++)
            {
                if (colliders[i].CompareTag("Bomb") || colliders[k].CompareTag("Bomb"))
                {
                    continue;
                }

                Physics2D.IgnoreCollision(colliders[i], colliders[k]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(
                this.gameObject.GetComponent<Collider2D>(),
                collision.gameObject.GetComponent<Collider2D>()
            );
        }
    }
}
