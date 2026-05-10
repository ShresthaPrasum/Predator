using Unity.VisualScripting;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        HingeJoint2D hingejoint = gameObject.GetComponent<HingeJoint2D>();
        
        if (collider.gameObject.CompareTag("BombTrigger"))
        {
            // Debug.LogWarning("Collider just got triggered");

            GameObject leftArm = collider.transform.parent.parent.Find("LowerLeftArm").gameObject;

            Rigidbody2D rigidbody = leftArm.GetComponent<Rigidbody2D>();

            hingejoint.connectedBody = rigidbody;

            gameObject.transform.SetParent(leftArm.transform);
            
            gameObject.transform.localPosition = new Vector3(3f, 1f, 0f);
        }
    }
}