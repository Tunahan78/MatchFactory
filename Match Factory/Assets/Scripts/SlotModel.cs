// Scripts/Model/SlotModel.cs
using System.Collections.Generic;
using System.Linq;

public class SlotModel
{
    // Seçilen ItemData'ları tutan saf C# listesi (Veri)
    private List<ItemData> itemsInSlot = new List<ItemData>();
    
    // Slot alanında tutulabilecek maksimum nesne sayısı (Örneğin 7)
    public const int MaxSlots = 7; 
    
    public int CurrentCount => itemsInSlot.Count;

    // Item'ı listeye ekle
    public void AddItem(ItemData data)
    {
        if (CurrentCount < MaxSlots)
        {
            itemsInSlot.Add(data);
        }
    }
    
    // Eşleştirme kontrolü için veriyi dışarıya güvenli bir şekilde sunar
    public List<ItemData> GetItems()
    {
        return itemsInSlot;
    }

    // Eşleşen nesneleri listeden kaldırır (Aşama 4'e hazırlık)
    public void RemoveMatchedItems(int matchedID)
    {
        // ... Kaldırma mantığı (Aşama 4'te detaylandırılacak) ...

        // Eşleşen ID'ye sahip olmayan öğeleri tutan yeni bir liste oluştur
        List<ItemData> newItemsList = itemsInSlot
            .Where(item => item.ID != matchedID)
            .ToList();

        // Mevcut listeyi, temizlenmiş yeni listeyle değiştir.
        itemsInSlot = newItemsList;
    }
    
}
