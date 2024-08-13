using UnityEngine;

public class Grabable : MonoBehaviour
{
    public bool bringable;
    public bool grabbed;

    Transform grabPoint;

    Vector3 velocity;

    public void Grab(Transform grabPoint)
    {
        if (grabbed) return;
        this.grabPoint = grabPoint;
        grabbed = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().freezeRotation = true;
    }

    public void UnGrab()
    {
        if (!grabbed) return;
        grabbed = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        if(TryGetComponent<Explosive>(out Explosive e))
        {
            StartCoroutine(e.Explode());
        }
    }

    private void Update()
    {
        if(grabbed)
        {
            transform.position = Vector3.SmoothDamp(transform.position, grabPoint.position, ref velocity, 0.05f);
        }
    }
}
