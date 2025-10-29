// Scripts/View/ItemView.cs
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer; // Nesnenin rengini değiştirmek için
    [SerializeField] private Rigidbody itemRigidbody;
    [SerializeField] private Collider itemCollider;
    
    public ItemData Data { get; private set; } // Aşama 0'dan gelen veri
    
    // Aşama 0: Data ataması
    public void Initialize(ItemData data)
    {
        Data = data;
        // ... Veriye göre görsel ayarlamalar (model yükleme, texture vb.) ...
    }

    // Unity'nin tıklama algılama metodu (Collider gerekli!)
    private void OnMouseDown()
    {
        // Tıklama olayını, ne yapılacağını bilmeden Presenter'a iletiyor.
        SelectionPresenter.Instance.HandleSelection(this);
    }

    // Sorumluluk: Sadece kendi görsel geri bildirimini yönetir.
    public void SetSelectionHighlight(bool isSelected)
    {
        if (meshRenderer != null)
        {
            // Basit bir test: Seçilirse sarı, seçilmezse normal rengine dönsün.
            meshRenderer.material.color = isSelected ? Color.yellow : Color.white;
        }
    }

    public void PrepareForSlot()
    {
        // 1. Seçilemez yap
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // 2. Yer çekiminden etkilenmesin
        if (itemRigidbody != null)
        {
            itemRigidbody.isKinematic = true;
             // itemRigidbody.linearVelocity = Vector3.zero;
            //  itemRigidbody.angularVelocity = Vector3.zero;
        }

        // 3. Boyutunu küçült (Örnek boyut, ihtiyaca göre ayarlanmalı)
        // Bu işlemi smooth hale getirmek için Coroutine de kullanılabilir, ancak MVP için direkt atama:
        
        const float slotScale = 0.055f; // Sizin önerdiğiniz aralıkta sabit bir değer
        transform.localScale = Vector3.one * slotScale;
    }
}