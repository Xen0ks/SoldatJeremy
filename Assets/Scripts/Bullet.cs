using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 forceToAdd = (transform.position - collision.transform.position).normalized;
            rb.AddForce(forceToAdd * 6000f, ForceMode.Impulse);
        }
    }
}
