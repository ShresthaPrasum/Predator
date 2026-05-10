using Unity.VisualScripting;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    public GameObject Bomb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BombTrigger"))
        {         
            Bomb.transform.SetParent(other.transform.parent);
        }
    }


}