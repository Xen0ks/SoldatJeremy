using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player p))
        {
            p.SetRespawnPoint(transform.position);
            Destroy(gameObject);
        }
    }
}
