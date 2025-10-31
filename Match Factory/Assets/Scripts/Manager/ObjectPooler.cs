// Scripts/System/ObjectPooler.cs

using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Dictionary ile çalışmak için

public class ObjectPooler : MonoBehaviour
{
    // Singleton Erişimi
    public static ObjectPooler Instance { get; private set; }

    [Header("Pool Ayarları")]
    [Tooltip("Havuzlanacak ItemData'ların listesi (SimpleItemSpawner'dakinin aynısı olmalı).")]
    [SerializeField] private ItemData[] itemTypesToPool;
    
    [Tooltip("Her bir Item türünden başlangıçta kaç adet oluşturulacağı.")]
    [SerializeField] private int poolSizePerType = 15;

    // Ana Veri Yapısı: Item ID'sine göre gruplanmış pasif ItemView listesi
    private Dictionary<int, List<ItemView>> itemPools = new Dictionary<int, List<ItemView>>();

    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
            // Sahne değiştirdiğinde yok olmasın (opsiyonel)
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Oyun başlarken havuzları doldur
        PopulatePools();
    }

    /// <summary>
    /// Oyun başladığında tüm ItemData türleri için havuzları doldurur.
    /// </summary>
    private void PopulatePools()
    {
        if (itemTypesToPool == null || itemTypesToPool.Length == 0)
        {
            Debug.LogError("[ObjectPooler] ItemTypesToPool listesi boş. ItemData'larınızı atayın!");
            return;
        }

        foreach (ItemData data in itemTypesToPool)
        {
            // 1. O Item ID'si için yeni bir havuz listesi oluştur
            List<ItemView> newPool = new List<ItemView>();
            itemPools.Add(data.ID, newPool);

            // 2. Belirlenen adet kadar nesneyi Instantiate et
            for (int i = 0; i < poolSizePerType; i++)
            {
                // ItemData'nın içindeki Prefab'i kullanarak nesneyi oluştur
                GameObject itemGO = Instantiate(data.Prefab, transform);
                
                ItemView itemView = itemGO.GetComponent<ItemView>();
                
                // ItemView'i Model verisiyle başlat (Çok önemli!)
                itemView.Initialize(data); 

                // 3. Nesneyi pasif hale getir ve havuza ekle
                itemGO.SetActive(false);
                newPool.Add(itemView);
            }
            Debug.Log($"Pooler: ID {data.ID} ({data.ItemName}) için {poolSizePerType} adet havuzlandı.");
        }
    }

    //------------------------------------------------------------
    // ⬇️ Public Metotlar: Item Alma ve İade Etme
    //------------------------------------------------------------
    
    /// <summary>
    /// Belirtilen Item ID'si için havuzdan aktif bir ItemView döndürür.
    /// </summary>
    public ItemView GetItem(int itemID)
    {
        if (!itemPools.ContainsKey(itemID))
        {
            Debug.LogError($"Pooler: ID {itemID} için havuz bulunamadı!");
            return null;
        }

        List<ItemView> pool = itemPools[itemID];
        ItemView itemToReturn = null;
        
        // Havuzda pasif durumda olan ilk nesneyi bul
        itemToReturn = pool.FirstOrDefault(item => !item.gameObject.activeInHierarchy);

        // Eğer havuzda pasif nesne kalmadıysa (Havuz Büyümesi)
        if (itemToReturn == null)
        {
            // Orijinal prefab'i bularak havuzu dinamik olarak büyüt
            ItemData data = itemTypesToPool.FirstOrDefault(d => d.ID == itemID);
            if (data != null)
            {
                Debug.LogWarning($"Pooler: ID {itemID} için havuz büyütülüyor.");
                GameObject newItemGO = Instantiate(data.Prefab, transform);
                itemToReturn = newItemGO.GetComponent<ItemView>();
                itemToReturn.Initialize(data);
                pool.Add(itemToReturn);
            }
            else
            {
                Debug.LogError($"Pooler: ID {itemID} için büyüme başarısız. ItemData bulunamadı.");
                return null;
            }
        }
        
        // Item'ı aktif hale getir ve döndür
        itemToReturn.gameObject.SetActive(true);
        return itemToReturn;
    }

    /// <summary>
    /// ItemView'i kullanımdan sonra pasif hale getirir ve havuza iade eder.
    /// </summary>
    public void ReturnToPool(ItemView item)
    {
        // 1. Aktifliği kapat
        item.gameObject.SetActive(false);
        
        // 2. Item'ı havuzun altına yerleştir (Hiyerarşi temizliği için)
        item.transform.SetParent(this.transform); 
    }
}