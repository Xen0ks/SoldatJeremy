using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBehaviour : MonoBehaviour
{
    [SerializeField] float stickRange;
    [SerializeField] LayerMask whatIsStickable;
    Camera cam;
    bool isSticking;
    PlayerController pc;
    Rigidbody rb;

    Vector3 stickPoint;

    RaycastHit hit;

    private void Start()
    {
        cam = Camera.main;
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (CanStick())
        {
            stickPoint = hit.point;
            Stick();
        }



        if (isSticking)
        {
            // Arr�te le mouvement si le joueur est proche du point de stick
            if (Vector3.Distance(transform.position, stickPoint) < 1.5f)
            {
                StopMovement();
            }

            // D�tache si la touche LeftControl est rel�ch�e
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                UnStick();
            }
        }
    }

    void Stick()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        // Applique une force vers le point de stick
        Vector3 direction = (hit.point - transform.position).normalized;
        rb.AddForce(direction * 250f, ForceMode.Impulse);

        isSticking = true;
        pc.enabled = false;
    }

    void StopMovement()
    {
        // Immobilise le Rigidbody
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;  // D�sactive la physique pour emp�cher tout mouvement
        if (hit.transform.TryGetComponent<Platform>(out Platform p))
        {
            stickPoint = p.transform.position;
        }
        // Positionner le joueur l�g�rement en avant du point de stick pour �viter le clipping
        transform.position = stickPoint + hit.normal * 0.5f; // 0.5f est la distance � ajuster, ajustez-la selon vos besoins
    }

    void UnStick()
    {
        isSticking = false;
        pc.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;  // R�active la physique pour permettre au joueur de bouger � nouveau
    }

    bool CanStick()
    {
        // Raycast pour d�tecter les surfaces dans le LayerMask sp�cifi�
        if (Input.GetKeyDown(KeyCode.LeftControl) && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, stickRange, whatIsStickable))
        {
            // V�rifie si la surface est suffisamment verticale
            Vector3 surfaceNormal = hit.normal;
            float angle = Vector3.Angle(surfaceNormal, Vector3.up);

            // On consid�re la surface comme verticale si l'angle est proche de 90 degr�s
            if (angle > 80f && angle < 100f) // Ajustez cette plage d'angle selon vos besoins
            {
                return true;
            }
        }
        return false;
    }
}