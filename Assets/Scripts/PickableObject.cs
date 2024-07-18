using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public string name;
    public int hitDamages;
    public int throwDamages;
    public Vector3 posOnArm;
    public Quaternion rotOnArm;
    public bool inHand = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<InteractBehaviour>(out InteractBehaviour interactBehaviour))
        {
            
        }
    }
}

