using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpBooster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float jump = 1f;

    public float Duration = 3f;

    private readonly Dictionary<Movement, float> _originialJump = new();

    private readonly Dictionary<Movement, Coroutine> _resetRoutines = new();

    public AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
       audioSource = gameObject.AddComponent<AudioSource>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        if (collision.CompareTag("BombTrigger"))
        {
            var movement = collision.gameObject.transform.parent.parent.GetComponent<Movement>();

            if(movement == null)
            {
                return;
            }

            audioSource.PlayOneShot(audioClip);

            if(movement.movementParticleSystem!= null)
            {
                movement.movementParticleSystem.Play();
            }

            if (!_originialJump.ContainsKey(movement))
            {
                _originialJump[movement] = movement.jumpForce;
            }

            movement.jumpForce = jump;

            if(_resetRoutines.TryGetValue(movement, out var routine)&& routine != null)
            {
                StopCoroutine(routine);
            }

            _resetRoutines[movement] = StartCoroutine(ResetJumpAfterDelay(movement,Duration));

            foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = false;
            }

            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            ParticleSystem.Stop();
        }
    }
    private IEnumerator ResetJumpAfterDelay(Movement movement, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (movement != null && _originialJump.TryGetValue(movement, out var originalJump))
        {
            movement.jumpForce = originalJump;
        }

        _originialJump.Remove(movement);
        _resetRoutines.Remove(movement);
    }
}