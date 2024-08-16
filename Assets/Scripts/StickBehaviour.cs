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
            // Arrête le mouvement si le joueur est proche du point de stick
            if (Vector3.Distance(transform.position, stickPoint) < 1.5f)
            {
                StopMovement();
            }

            // Détache si la touche LeftControl est relâchée
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
        rb.isKinematic = true;  // Désactive la physique pour empêcher tout mouvement
        if (hit.transform.TryGetComponent<Platform>(out Platform p))
        {
            stickPoint = p.transform.position;
        }
        // Positionner le joueur légèrement en avant du point de stick pour éviter le clipping
        transform.position = stickPoint + hit.normal * 0.5f; // 0.5f est la distance à ajuster, ajustez-la selon vos besoins
    }

    void UnStick()
    {
        isSticking = false;
        pc.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;  // Réactive la physique pour permettre au joueur de bouger à nouveau
    }

    bool CanStick()
    {
        // Raycast pour détecter les surfaces dans le LayerMask spécifié
        if (Input.GetKeyDown(KeyCode.LeftControl) && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, stickRange, whatIsStickable))
        {
            // Vérifie si la surface est suffisamment verticale
            Vector3 surfaceNormal = hit.normal;
            float angle = Vector3.Angle(surfaceNormal, Vector3.up);

            // On considère la surface comme verticale si l'angle est proche de 90 degrés
            if (angle > 80f && angle < 100f) // Ajustez cette plage d'angle selon vos besoins
            {
                return true;
            }
        }
        return false;
    }
}