using UnityEngine;

public class SlotView : MonoBehaviour
{
    [SerializeField] private Transform itemAnchor;
    //  Slot'a yerleştirilen Item'ın referansı
    public ItemView CurrentItem { get; private set; } 

    public bool IsOccupied => CurrentItem != null; // Dolu mu?

    // Item'ı yuvaya görsel olarak yerleştiren metot
    public void PlaceItem(ItemView item)
    {
        CurrentItem = item;
        
        item.PrepareForSlot();
        // Hiyerarşi ve Konum Ayarlaması
        item.transform.SetParent(itemAnchor); // SetParent kullanmak daha iyidir
        item.transform.localPosition = Vector3.zero; 
        
        item.SetSelectionHighlight(false);
    }

    // Item'ı yuvadan kaldıran metot
    public void RemoveItem()
    {
        CurrentItem.transform.parent = null; // Hiyerarşiden çıkar
        CurrentItem = null;
    }
}
