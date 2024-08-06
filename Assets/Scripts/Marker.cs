using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [HideInInspector]
    public MarkerData markerData;

    public void SetMarker(MarkerData markerData)
    {
        this.markerData = markerData;
        GetComponent<Image>().sprite = markerData.icon;
        GetComponent<Button>().onClick.AddListener(delegate { MapSystem.instance.SelectMarker(this); });
    }
}
