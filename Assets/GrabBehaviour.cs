using System.Collections;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour
{
    [SerializeField] Transform camera;

    [SerializeField] float grabRange;

    [SerializeField] LayerMask whatIsGrabable;

    [SerializeField] Transform boneToMove;
    [SerializeField] Transform boneOriginalPos;

    Grabable grabable;
    bool grabing;
    bool throwing;
    bool canThrow = true;
    Vector3 velocity;

    private void Update()
    {
        RaycastHit hit;


        if (Physics.Raycast(camera.position, camera.forward, out hit, grabRange, whatIsGrabable) && Input.GetKey(KeyCode.Mouse1) && !grabing && canThrow)
        {
            canThrow = false;
            bool isGrabable = hit.transform.TryGetComponent<Grabable>(out grabable);
            throwing = true;
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, hit.point, ref velocity, 0.3f);
            if (grabable != null && grabable.bringable)
            {
                StartCoroutine(Grab());
                Debug.Log("Start Grabbing");
            }
        }
        else if(canThrow)
        {
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, boneOriginalPos.position, ref velocity, 0.1f);
            Invoke("UnGrab", 0.1f);
        }
    }

    IEnumerator Grab()
    {
        if (grabing) yield return null;
        yield return new WaitForSeconds(0.1f);
        grabable.Grab(boneToMove);
        grabing = true;
        yield return new WaitForSeconds(1f);
        UnThrow();
    }

    void UnThrow()
    {
        canThrow = true;
    }

    void UnGrab()
    {
        if (!grabing)
        {
            return;
        }
        StopCoroutine(Grab());
        grabing = false;
        throwing = false;
        if (grabable != null) grabable.UnGrab();
        Debug.Log("Stop Grabbing");
    }
}
