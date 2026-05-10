using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp: MonoBehaviour
{
    public float Speed = 7f;
    public float Duration = 3f;

    private readonly Dictionary<Movement, float> _originalSpeeds = new();
    private readonly Dictionary<Movement, Coroutine> _resetRoutines = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var movement = other.gameObject.GetComponent<Movement>();
            if (movement == null)
            {
                return;
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