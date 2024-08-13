using System.Collections;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour
{
    // References
    [SerializeField] Transform camera;
    [SerializeField] float grabRange;
    [SerializeField] LayerMask whatIsGrabable;
    [SerializeField] Transform boneToMove;
    [SerializeField] Transform boneOriginalPos;
    [SerializeField] float throwDistance = 1.0f; // Distance à laquelle l'objet sera jeté
    [SerializeField] float moveSpeed = 5f; // Vitesse à laquelle le joueur se déplace vers l'objet
    [SerializeField] float stopDistance = 0.1f; // Distance à laquelle le mouvement s'arrête en douceur
    [SerializeField] GameObject arm;
    [SerializeField] Transform grabPoint;
    Animator armAnim;

    private Rigidbody rb;
    private Grabable grabable;
    [HideInInspector] public bool grabbing;
    private bool justThrowed;
    [HideInInspector] public bool dashing;
    private Vector3 velocity;
    private float smoothTime = 0.1f; // Temps de lissage
    private Vector3 dashTarget;
    private RaycastHit hit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        armAnim = arm.GetComponent<Animator>();
    }

    private void Update()
    {
        if (CanGrab())
        {
            armAnim.SetBool("Show", true);

            // Déplacer le bras vers l'objet pointé
            MoveBone(1);

            // Vérifier si l'objet est attrapable
            if (hit.transform.TryGetComponent<Grabable>(out grabable))
            {
                if (grabable.bringable)
                {
                    StartCoroutine(Grab());
                }
                else
                {
                    dashTarget = hit.point;
                    Invoke("StartDash", 0.11f);
                }

            }
        }
        else
        {
            if (grabbing)
            {
                // Ramener le bras à la position de grab
                MoveBone(3);

            }
            else
            {
                // Si le clic droit est relâché et que le bras n'est pas en train d'attraper, ramener le bras à sa position originale
                MoveBone(0);
            }
        }

        if (!Input.GetKey(KeyCode.Mouse1))
        {
            justThrowed = false;
            dashing = false;
            UnGrab();
        }
        if (dashing)
        {
            MovePlayerTowards();
        }
    }

    void StartDash()
    {
        dashing = true;
        justThrowed = true;
    }

    IEnumerator Grab()
    {
        if (grabbing) yield break; // Sortir de la coroutine si déjà en train d'attraper

        yield return new WaitForSeconds(smoothTime * 2);
        grabbing = true;

        // Attacher l'objet au bras
        grabable?.Grab(boneToMove);
    }

    void MoveBone(int state)
    {
        if(state == 0)
        {
            Util.instance.SetLayerRecursively(arm, 7);
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, boneOriginalPos.position, ref velocity, smoothTime);
        }
        if(state == 1)
        {
            Util.instance.SetLayerRecursively(arm, 0);
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, hit.point, ref velocity, smoothTime);
        }
        if(state == 2)
        {
            Util.instance.SetLayerRecursively(arm, 0);
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, dashTarget, ref velocity, smoothTime);
        }
        if(state == 3)
        {
            Util.instance.SetLayerRecursively(arm, 0);
            boneToMove.position = Vector3.SmoothDamp(boneToMove.position, grabPoint.position, ref velocity, smoothTime);
        }
    }

    void UnGrab()
    {
        if (!grabbing) return;

        grabbing = false;
        grabable?.UnGrab();
        grabable = null;
        justThrowed = true;
        dashing = false;
        rb.useGravity = true;
    }

    public bool CanGrab()
    {
        return Physics.Raycast(camera.position, camera.forward, out hit, grabRange, whatIsGrabable) &&
               Input.GetKey(KeyCode.Mouse1) &&
               !grabbing &&
               Vector3.Distance(transform.position, hit.transform.position) > throwDistance &&
               !justThrowed 
               && !dashing;
    }

    void MovePlayerTowards( )
    {
        // Calculer la distance restante
        float distanceToTarget = Vector3.Distance(transform.position, dashTarget);

        // Déplacer le bras vers la target

        if(distanceToTarget > 5)
        {
            MoveBone(2);

        }
        else
        {
            MoveBone(0);
        }

        // Déplacer le joueur vers la position cible
        transform.position = Vector3.Lerp(transform.position, dashTarget, Time.deltaTime * moveSpeed / distanceToTarget);

        // Si la distance restante est inférieure à la distance d'arrêt, ralentir le mouvement
        if (distanceToTarget < 2)
        {
            rb.velocity = Vector3.zero;
            dashing = false; // Arrêter le dash une fois la cible atteinte

        }
    }
}
