using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    int collCount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            return;
        }
        collCount++;
        if(collCount == 1)
        {
            transform.position -= new Vector3(0, 0.21f, 0);
            onEvent.Invoke();
            GetComponent<BoxCollider>().size = new Vector3(1.742998f, 0.5826989f, 1.742998f);
            GetComponent<BoxCollider>().center = new Vector3(0, 0.3147853f, 0);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            return;
        }
        collCount--;

        if(collCount <= 0)
        {
            collCount = 0;
            transform.position += new Vector3(0, 0.21f, 0);
            offEvent.Invoke();
            GetComponent<BoxCollider>().size = new Vector3(1.742998f, 0.3752458f, 1.742998f);
            GetComponent<BoxCollider>().center = new Vector3(0, 0.2171199f, 0);

        }
    }
}
