using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player p))
        {
            p.Die();
        }

        if(other.transform.parent != null && other.transform.parent.TryGetComponent<Enemy>(out Enemy e))
        {
            e.Die();
        }
    }
}
