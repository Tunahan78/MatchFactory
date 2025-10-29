// Scripts/Presenter/MatchPresenter.cs
using UnityEngine;
using System.Linq; // LINQ kullanmak için gerekli
using System.Collections.Generic;

public class MatchPresenter : MonoBehaviour
{
    public static MatchPresenter Instance { get; private set; } 

    private void Awake()
    {
        Instance = this;
    }

    // SlotModel'den gelen veriyi kontrol eden ana metot
    public bool CheckForMatch(List<ItemData> itemsInSlot)
    {
        if (itemsInSlot.Count < 3)
        {
            return false; // Eşleştirme için en az 3 nesne olmalı
        }

        // LINQ (Language Integrated Query) kullanarak gruplama ve sayım yapıyoruz.
        // Bu, C#'ta en temiz ve verimli yoldur:
        var matchedGroups = itemsInSlot
            .GroupBy(item => item.ID) // ItemData'nın ID'sine göre grupla
            .Where(group => group.Count() >= 3) // Sayısı 3 veya daha fazla olan grupları filtrele
            .ToList();

        if (matchedGroups.Any())
        {
            // Eşleşme bulundu!
            int matchedID = matchedGroups.First().Key;
            
            Debug.Log($"🎉 Eşleşme Bulundu! ID: {matchedID} türünden 3 veya daha fazla nesne var.");

            // Aşama 5'i (Yok Etme) tetikle:
            SlotManager.Instance.HandleMatchFound(matchedID);
            
            return true;
        }

        return false;
    }
}