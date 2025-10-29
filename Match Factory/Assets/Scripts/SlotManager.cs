// Scripts/Manager/SlotManager.cs
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

        // 2. Model'i temizle
        model.RemoveMatchedItems(matchedID);

        // 3. SlotView'leri görsel olarak yeniden düzenle (Aşama 5)
    }

}
