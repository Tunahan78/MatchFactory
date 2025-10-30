// Scripts/Manager/SlotManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager Instance { get; private set; } // Singleton

    [Header("Slot Ayarları:")]
    public SlotView[] allSlotViews; // Inspector'dan Slot_0, Slot_1 vb. buraya atanacak.

    private SlotModel model = new SlotModel();

    private void Awake()
    {
        Instance = this;
    }

    // Seçilen ItemView'i işleyen merkezi metod
    public bool TryPlaceItem(ItemView item)
    {
        if (model.CurrentCount >= SlotModel.MaxSlots)
        {
            Debug.LogWarning("Slot alanı dolu! Oyuncu kaybeder.");
            // Burada oyun kaybetme/hata mantığı tetiklenecek.
            return false;
        }

        // 1. Veriyi Model'e kaydet (Model'in sorumluluğu)
        model.AddItem(item.Data);

        // 2. Görseli SlotView'e yerleştir (View'in sorumluluğu)
        SlotView targetSlot = allSlotViews[model.CurrentCount - 1]; // Yeni eklenen item en son slotu doldurur
        targetSlot.PlaceItem(item);

        // 3. Eşleştirme kontrolünü tetikle (Aşama 4'e geçiş)
        MatchPresenter.Instance.CheckForMatch(model.GetItems());

        return true;
    }

    // Yeni: MatchPresenter'dan gelen eşleşme sinyalini işleyen metot
    public void HandleMatchFound(int matchedID)
    {
        // 1. Görsel temizliği başlat (Aşama 5)
        // Eşleşen ItemView'leri bul ve yok etme sürecini başlat
        DestroyMatchedViews(matchedID);

        // 2. Model'i temizle
        model.RemoveMatchedItems(matchedID);

        // 3. SlotView'leri görsel olarak yeniden düzenle (Aşama 5)
        RelocateRemainingViews();
    }

    private void RelocateRemainingViews()
    {

        // Kalan ItemView'leri (View) sırayla alıp Model'deki yeni pozisyonlarına (SlotView'lere) yerleştireceğiz
        // Önce tüm SlotView'leri View listesinden alıyoruz
        List<ItemView> remainingItems = new List<ItemView>();
        foreach (SlotView slot in allSlotViews)
        {
            if (slot.CurrentItem != null)
            {
                remainingItems.Add(slot.CurrentItem);
                slot.RemoveItem(); // Eski referansı temizle
            }
        }
        // Kalan ItemView'leri yeni, boş slotlara sırayla yerleştir
        for (int i = 0; i < remainingItems.Count; i++)
        {
            SlotView targetSlot = allSlotViews[i];
            ItemView item = remainingItems[i];

            // SlotView'in PlaceItem metodunu kullanarak item'ı yeni yuvaya yerleştir
            targetSlot.PlaceItem(item);

            // Item'ın görsel olarak kayma animasyonu/geçişi burada tetiklenebilir.
        }
    }
    

    private void DestroyMatchedViews(int matchedID)
    {
        foreach(SlotView slotView in allSlotViews)
        {
            if (slotView.IsOccupied && slotView.CurrentItem.Data.ID == matchedID)
            {
                // SlotView'den ItemView referansını al
                ItemView itemToDestroy = slotView.CurrentItem;

                // Slot'un referansını temizle (önemli)
                slotView.RemoveItem();

                // ItemView'in yok olma/pool'a dönme animasyonunu başlat
                // Şimdilik basitçe Destroy() kullanıyoruz (Aşama 6'da Object Pooling'e geçilebilir).
                Destroy(itemToDestroy.gameObject);
            }
            
        }
    }
}
