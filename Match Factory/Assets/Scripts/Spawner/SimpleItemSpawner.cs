// Scripts/Test/SimpleItemSpawner.cs
using UnityEngine;
using System.Linq;

public class SimpleItemSpawner : MonoBehaviour
{
    [Header("Item Data Ayarları")]
    [Tooltip("Sahnede oluşturulacak ItemData ScriptableObject'lerinin listesi.")]
    public ItemData[] itemTypes; 

    [Header("Spawn Ayarları")]
    [Tooltip("Tahtada oluşturulacak toplam nesne sayısı.")]
    public int spawnCount = 50; 

    [Tooltip("Nesnelerin rastgele dağıtılacağı 3D alanın boyutları.")]
    public Vector3 spawnAreaSize = new Vector3(8f, 3f, 8f);

    void Start()
    {
        SpawnItemsForTest();
    }

    /// <summary>
    /// ItemData'ları kullanarak sahnede ItemView nesneleri oluşturur ve onlara veriyi bağlar.
    /// </summary>
    void SpawnItemsForTest()
    {
        // Temel kontrol: Atama yapılıp yapılmadığını kontrol et
        if (itemTypes == null || itemTypes.Length == 0 || itemTypes.Any(data => data == null))
        {
            Debug.LogError("SimpleItemSpawner: ItemTypes listesi boş veya null referans içeriyor. ItemData SO'larınızı atayın!");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // 1. Rastgele bir ItemData seç
            ItemData selectedData = itemTypes[Random.Range(0, itemTypes.Length)];

            // 2. Item Prefab'ini oluştur (Prefab, ItemData'nın içinde tanımlı)
            // ItemView ve Collider'ın prefab'e bağlı olduğundan emin olun.
            GameObject newItemGO = Instantiate(
                selectedData.Prefab, 
                GetRandomSpawnPosition(), 
                Random.rotation // Başlangıçta rastgele bir dönme açısı veriyoruz
            );

            // 3. ItemView'i al ve Data'yı bağla! (Bağımlılık Enjeksiyonu)
            ItemView itemView = newItemGO.GetComponent<ItemView>();
            if (itemView != null)
            {
                // ItemView'in Initialize metodu ile veriyi atıyoruz.
                itemView.Initialize(selectedData); 
            }
            else
            {
                Debug.LogError($"HATA: Spawn edilen prefab'de ItemView bileşeni bulunamadı: {selectedData.ItemName}. Lütfen prefab'i kontrol edin.");
            }
        }
        
        Debug.Log($"Tahtaya toplam {spawnCount} adet rastgele nesne başarıyla yerleştirildi.");
    }

    /// <summary>
    /// Belirlenen alan içinde rastgele bir pozisyon hesaplar.
    /// </summary>
    Vector3 GetRandomSpawnPosition()
    {
        // Nesnelerin yığılması için rastgele bir alan ve y ekseninde yerden başlama
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float y = Random.Range(1f, spawnAreaSize.y); 
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        
        return new Vector3(x, y, z);
    }
}