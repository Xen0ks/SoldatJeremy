using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    // References

    [SerializeField] GameObject mapUI;
    [SerializeField] Transform mapDisplay;

    [SerializeField] MarkerData[] existingMarkers;

    [SerializeField] GameObject markerButtonPrefab;
    [SerializeField] Transform markerButtonsParent;
    [SerializeField] Animator markersPanelAnim;

    [SerializeField] GameObject markerPrefab;
    [SerializeField] Transform markersParent;



    // Fonctionnement
    bool isOpen;
    Marker selectedMarker;


    public static MapSystem instance;
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        InitializeMarkerButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMap();
        }

        if (isOpen)
        {
            // Gestion du zoom de la minimap
            float newSize = mapDisplay.localScale.x + (Input.mouseScrollDelta.y * 0.5f);
            if (newSize <= 10 && newSize > 1)
            {
                mapDisplay.localScale = new Vector2(newSize, newSize);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                PlaceMarker(Input.mousePosition);
            }
        }
    }

    public void PlaceMarker(Vector3 pos)
    {
        Marker marker = Instantiate(markerPrefab, markersParent).GetComponent<Marker>();
        marker.transform.position = pos;
        marker.SetMarker(existingMarkers[0]);
        SelectMarker(marker);
    }

    public void SelectMarker(Marker marker)
    {
        markersPanelAnim.SetBool("Show", true);
        selectedMarker = marker;
    }

    public void ChangeMarker(MarkerData m)
    {
        if (selectedMarker == null) return;

        selectedMarker.SetMarker(m);
        selectedMarker = null;
        markersPanelAnim.SetBool("Show", false);
    }

    public void DeleteMarker()
    {
        if (selectedMarker == null) return;

        Destroy(selectedMarker.gameObject);
        selectedMarker = null;
        markersPanelAnim.SetBool("Show", false);
    }





    public void ToggleMap()
    {
        mapUI.SetActive(!mapUI.activeSelf);
        isOpen = mapUI.activeSelf;

        if(isOpen)
        {
            Util.instance.OpenMenu();
        }
        else
        {
            Util.instance.CloseMenu();
        }
    }

    void InitializeMarkerButtons()
    {
        foreach(MarkerData m in existingMarkers)
        {
            Transform spawnedMarker = Instantiate(markerButtonPrefab, markerButtonsParent).transform;
            // spawnedMarker.GetComponent<Text>().text = existingMarkers[i].name;
            spawnedMarker.GetChild(0).GetComponent<Image>().sprite = m.icon;
            spawnedMarker.GetComponent<Button>().onClick.AddListener(delegate { ChangeMarker(m); });
        }
    }
}
