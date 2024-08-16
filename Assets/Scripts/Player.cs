using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player : MonoBehaviour
{
    Vector3 respawnPoint;

    public Transform head;



    public static Player instance;
    void Awake()
    {
        instance = this;
    }

    public void Die()
    {
        transform.position = respawnPoint;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GrabBehaviour gb =  GetComponent<GrabBehaviour>();
        gb.justThrowed = false;
        gb.dashing = false;
        gb.UnGrab();

    }

    public void SetRespawnPoint(Vector3 rp)
    {
        respawnPoint = rp;
    }
}
