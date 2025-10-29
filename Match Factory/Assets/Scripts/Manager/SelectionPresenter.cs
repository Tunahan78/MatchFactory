// Scripts/Presenter/SelectionPresenter.cs
using UnityEngine;

public class SelectionPresenter : MonoBehaviour
{
    // Singleton deseni ile kolay erişim (MVP için hızlı bir yol)
    public static SelectionPresenter Instance { get; private set; } 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Tıklanan öğeyi ItemView'den alan merkezi metod
    public void HandleSelection(ItemView selectedItem)
    {
        // 1. Geri Bildirim: Seçilen nesneye geçici bir renk verelim (TEST AMAÇLI)
        // Görünüm işini ItemView'e delege edebiliriz:
        selectedItem.SetSelectionHighlight(true);
        bool placedSuccessfully = SlotManager.Instance.TryPlaceItem(selectedItem);

        if (placedSuccessfully)
        {
            Debug.Log("göndedrme başarılı");
        }
       

        // Buraya Aşama 3'te taşıma ve SlotModel'e ekleme mantığı gelecek.
    }
}