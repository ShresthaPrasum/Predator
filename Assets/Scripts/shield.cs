using System.Collections;
using UnityEngine;

public class shield : MonoBehaviour
{
    [SerializeField] private float reactivateDelay = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame

    public AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
       audioSource = gameObject.AddComponent<AudioSource>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();

        if (collision.gameObject.CompareTag("BombTrigger"))
        {
            var movement = collision.gameObject.transform.parent.parent.GetComponent<Movement>();

            if (movement.movementParticleSystem != null)
            {
                movement.movementParticleSystem.Play();
            }

            audioSource.PlayOneShot(audioClip);

            collision.gameObject.SetActive(false);
            StartCoroutine(ReactivateAfterDelay(collision.gameObject));

            foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.enabled = false;
            }


            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            ParticleSystem.Stop();
        }
    }

    private IEnumerator ReactivateAfterDelay(GameObject target)
    {
        yield return new WaitForSeconds(reactivateDelay);

        if (target != null)
        {
            target.SetActive(true);
        }
    }

}
