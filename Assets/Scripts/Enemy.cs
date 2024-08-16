using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] Transform target;
    [SerializeField] Transform spine;
    [SerializeField] Transform gun;
    [SerializeField] Transform gunPoint;
    [SerializeField] float rotationSpeed = 4;
    [SerializeField] LayerMask whatIsObstacle;
    [SerializeField] float detectionRadius;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] UnityEvent dieEvent;
    Animator anim;

    NavMeshAgent navMesh;

    bool detected;
    bool shooting;
    bool stunned;
    bool dead;

    [Header("Ragdoll")]
    Collider mainCollider;
    [SerializeField] GameObject rig;
    Rigidbody rb;
    Vector3 respawnPos;

    [Header("Shooting")]
    [SerializeField] float shotCooldown = 0.4f;
    float lastShotTime;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        target = GameObject.FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        target = Player.instance.transform;
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<Collider>();
        Transform[] rigTransforms = GetComponentsInChildren<Transform>();

        UnStun();
    }

    private void Update()
    {
        if (dead) return;
        if (stunned)
        {
            anim.SetBool("chase", false);
            navMesh.SetDestination(transform.position);
            shooting = false;
            return;
        }

        rig.transform.position = transform.position;
        float distanceToTarget = Vector3.Distance(target.position, transform.position);

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // Vérifier si le joueur est dans le champ de vision et dans le rayon de poursuite
        if (angleToTarget <= 180f / 2 && distanceToTarget <= detectionRadius && distanceToTarget > 2f)
        {
            Vector3 spineTarget = target.position;
            spineTarget.y += 0.734f;
            spine.LookAt(spineTarget);

            detected = true;

            // Tourner l'ennemi vers la cible
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Déplacer l'ennemi vers la cible
            navMesh.SetDestination(target.position);
            anim.SetBool("chase", true);

            // Commencer à tirer si l'ennemi ne tire pas déjà et que le cooldown est écoulé
            if (Time.time >= lastShotTime + shotCooldown)
            {
                StartCoroutine(Shot());
            }
        }
        else
        {
            // Si le joueur n'est plus dans le champ de vision, arrêter la poursuite
            navMesh.SetDestination(transform.position);
            detected = false;
            shooting = false;
            anim.SetBool("chase", false);
        }

        // Positionner et orienter l'arme
        gun.position = gunPoint.position;
        gun.rotation = gunPoint.rotation;
    }

    IEnumerator Shot()
    {
        if (shooting) yield break;
        shooting = true;

        yield return new WaitForSeconds(0.4f); // Temporisation pour simuler le temps de tir
        lastShotTime = Time.time; // Enregistrer le temps du dernier tir

        Transform instantiatedBullet = Instantiate(bulletPrefab).transform;
        instantiatedBullet.position = gunPoint.position;
        instantiatedBullet.GetComponent<Rigidbody>().AddForce(spine.transform.forward * 5000f);

        shooting = false;

        Destroy(instantiatedBullet.gameObject, 2f);
    }

    public IEnumerator Stun(Vector3 kbDir)
    {
        if (dead) yield break ;
        Collider[] ragDollColliders = rig.GetComponentsInChildren<Collider>();
        Rigidbody[] rigidbodies = rig.GetComponentsInChildren<Rigidbody>();

        stunned = true;
        anim.enabled = false;
        mainCollider.enabled = false;
        GetComponent<Grabable>().enabled = false;
        rb.isKinematic = true;
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(kbDir * 50f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(4f);

        if (stunned)
        {
            UnStun();
        }
    }

    void UnStun()
    {
        Collider[] ragDollColliders = rig.GetComponentsInChildren<Collider>();
        Rigidbody[] rigidbodies = rig.GetComponentsInChildren<Rigidbody>();
        stunned = false;
        anim.enabled = true;
        mainCollider.enabled = true;

        rb.isKinematic = false;
        GetComponent<Grabable>().enabled = true;

        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

        respawnPos = rig.transform.position;
        transform.position = respawnPos;
        rig.transform.position = transform.position;
    }

    public void Die()
    {
        dead = true;
        dieEvent.Invoke();
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gunPoint.position, detectionRadius);
    }
}
