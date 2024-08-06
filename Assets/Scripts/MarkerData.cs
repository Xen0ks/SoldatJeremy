 using UnityEngine;

[CreateAssetMenu(fileName = "Marker", menuName = "ScriptableObjects/CreateMarker", order = 2)]
public class MarkerData : ScriptableObject
{
    public string name;
    public Sprite icon;
}

