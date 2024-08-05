using UnityEngine;

public class Grabable : MonoBehaviour
{
    public bool bringable;
    public bool grabbed;

    Transform grabPoint;


    public void Grab(Transform grabPoint)
    {
        if (grabbed) return;
        this.grabPoint = grabPoint;
        grabbed = true;
    }

    public void UnGrab()
    {
        if (!grabbed) return;
        grabbed = false;

    }

    private void Update()
    {
        if(grabbed)
        {
            transform.position = grabPoint.position;
        }
    }
}
