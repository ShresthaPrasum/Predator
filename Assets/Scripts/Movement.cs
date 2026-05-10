using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
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

    public float speed = 1.5f;
    [SerializeField] float stepWait = .5f;
    public float jumpForce = 10f;
    [SerializeField] float jumpCooldown = 0.2f;

    public Key rightKey = Key.D;
    public Key leftKey = Key.A;
    public Key jumpKey = Key.W;

    private bool isGrounded;
    private float nextJumpTime;

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
        if(Keyboard.current[rightKey].isPressed)
        {
            animator.Play("WalkRight");
            StartCoroutine(MoveRight(stepWait));
        }
        else if(Keyboard.current[leftKey].isPressed)
        {
            animator.Play("WalkLeft");
            StartCoroutine(MoveLeft(stepWait));
        }
        else
        {
            animator.Play("Idle");
        }    
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, positionRadius, ground);

        if (isGrounded && Keyboard.current[jumpKey].wasPressedThisFrame && Time.time >= nextJumpTime)
        {
            nextJumpTime = Time.time + jumpCooldown;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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