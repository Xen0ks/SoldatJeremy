using UnityEngine;

public class Explosable : MonoBehaviour
{
    public bool pushable = false;
    public bool destructible = false;
    [HideInInspector] public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
