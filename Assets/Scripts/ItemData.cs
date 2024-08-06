using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/CreateItem", order = 1)]
public class ItemData : ScriptableObject
{
    public string name;
    public Vector3 posOnArm;
    public Quaternion rotOnArm;
    public GameObject prefab;

    public ItemType itemType;

    [Header("Gun Variables")]
    public float range;
    public float damages;

    [Header("SpecialItem Variables")]


    [Header("ClassicObject Variables")]
    public int hitDamages;
    public int throwDamages;

}

public enum ItemType
{
    Guns,
    SpecialItem,
    ClassicObject
}
