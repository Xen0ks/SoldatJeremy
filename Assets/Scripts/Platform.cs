using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField]
    int platformType; // 0 : Normal 1: Allez retour 2: Falling
    public bool setEnfant = true;

    [SerializeField]
    Transform platformPrefab;

    // Allez Retour
    Transform currentPoint;
    Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField]
    Transform[] points;
    bool started = false;

    // Falling
    [HideInInspector] public Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
        rb = GetComponent<Rigidbody>();
        if(platformType == 1) currentPoint = points[0];
    }

    private void Update()
    {
        if (platformType != 1) return;
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentPoint.position) < 0.1f || currentPoint == null)
        {
            currentPoint = points[Random.Range(0, points.Length)];
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && setEnfant) collision.transform.SetParent(transform);
        if(platformType == 2 && collision.transform.tag != "Ground" && !started)
        {
            started = true;
            originalPos = transform.position;
            Invoke("Fall", 1.5f);
            Invoke("Instantiate", 3f);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == "Player") collision.transform.parent = null;
    }

    void Fall()
    {
        Debug.Log("Fall");
        rb.useGravity = true;
        Destroy(gameObject, 5f);
    }

    void Instantiate()
    {
        Transform newPlatform = Instantiate(platformPrefab);
        newPlatform.GetComponent<Rigidbody>().useGravity = false;
        newPlatform.position = originalPos;
        Platform newPlat = newPlatform.GetComponent<Platform>();
        newPlat.platformType = platformType;
        newPlat.points = points;
        newPlat.originalPos = originalPos;
    }


}
