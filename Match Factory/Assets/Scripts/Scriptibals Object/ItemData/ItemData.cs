using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game Data/Match 3D Item")]
public class ItemData : ScriptableObject
{
    public int ID;

    public string ItemName;

    public GameObject Prefab;
}
