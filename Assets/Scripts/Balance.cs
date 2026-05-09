using UnityEngine;

public class Balance : MonoBehaviour
{
    public float targetRotation;
    Rigidbody2D rb;
    public float force;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.deltaTime));
    }
}
