using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public GameObject leftLeg;

    public GameObject rightLeg;

    Rigidbody2D leftlegRb;

    Rigidbody2D rightlegRb;

    public Rigidbody2D rb;

    public Animator animator;

    [SerializeField] float speed = 1.5f;
    [SerializeField] float stepWait = .5f;
    [SerializeField] float jumpForce = 10f;

    private bool isGrounded;

    public float positionRadius;

    public LayerMask ground;

    public Transform groundCheck;

    void Start()
    {
        leftlegRb = leftLeg.GetComponent<Rigidbody2D>();

        rightlegRb = rightLeg.GetComponent<Rigidbody2D>();
    }

    void Update()
    {      
        if(Keyboard.current.dKey.isPressed)
        {
            animator.Play("WalkRight");
            StartCoroutine(MoveRight(stepWait));
        }
        else if(Keyboard.current.aKey.isPressed)
        {
            animator.Play("WalkLeft");
            StartCoroutine(MoveLeft(stepWait));
        }
        else
        {
            animator.Play("Idle");
        }    
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, positionRadius,ground);

        if((isGrounded == true && Keyboard.current.wKey.isPressed) || (isGrounded == true && Keyboard.current.spaceKey.isPressed))
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
    }

    IEnumerator MoveRight(float seconds)
    {
        leftlegRb.AddForce(Vector2.right* (speed*1000) * Time.deltaTime);
        yield return new WaitForSeconds(seconds);
        rightlegRb.AddForce(Vector2.right* (speed*1000) * Time.deltaTime);
    }
    IEnumerator MoveLeft(float seconds)
    {
        rightlegRb.AddForce(Vector2.left* (speed*1000) * Time.deltaTime);
        yield return new WaitForSeconds(seconds);
        leftlegRb.AddForce(Vector2.left* (speed*1000) * Time.deltaTime);
    }
}