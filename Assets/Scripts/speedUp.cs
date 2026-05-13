using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedUp: MonoBehaviour
{
    public float Speed = 7f;
    public float Duration = 3f;

    private readonly Dictionary<Movement, float> _originalSpeeds = new();
    private readonly Dictionary<Movement, Coroutine> _resetRoutines = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BombTrigger"))
        {
            var movement = other.gameObject.transform.parent.parent.GetComponent<Movement>();
            if (movement == null)
            {
                return;
            }

            if (movement.movementParticleSystem != null)
            {
                movement.movementParticleSystem.Play();
            }

            if (!_originalSpeeds.ContainsKey(movement))
            {
                _originalSpeeds[movement] = movement.speed;
            }

            movement.speed = Speed;

            if (_resetRoutines.TryGetValue(movement, out var routine) && routine != null)
            {
                StopCoroutine(routine);
            }

            _resetRoutines[movement] = StartCoroutine(ResetSpeedAfterDelay(movement, Duration));

            // foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            // {
            //     sprite.enabled = false;
            // }


            // gameObject.GetComponent<CircleCollider2D>().enabled = false;

        }
    }

    private IEnumerator ResetSpeedAfterDelay(Movement movement, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (movement != null && _originalSpeeds.TryGetValue(movement, out var originalSpeed))
        {
            movement.speed = originalSpeed;


        }

        _originalSpeeds.Remove(movement);
        _resetRoutines.Remove(movement);
    }
}