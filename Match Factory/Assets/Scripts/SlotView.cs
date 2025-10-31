using DG.Tweening;
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

        float duration = 0.25f; // Hareket süresi (saniye)

        // DOTween: Item'ı 0.25 saniyede Anchor pozisyonuna taşı.
        // 'Ease.OutBack' gibi bir eğri, hareketi daha canlı (sanki geri yaylanıyor gibi) yapar.
        item.transform.DOLocalMove(Vector3.zero, duration)
            .SetEase(Ease.OutSine); // Smooth bir eğri 

        item.SetSelectionHighlight(false);
    }
    

    // Item'ı yuvadan kaldıran metot
    public void RemoveItem()
    {
        CurrentItem.transform.parent = null; // Hiyerarşiden çıkar
        CurrentItem = null;
    }
}
