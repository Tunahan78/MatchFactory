// Scripts/Core/LevelGoal.cs

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemGoal
{
    [Tooltip("Hangi ItemData türünün eşleştirilmesi gerekiyor.")]
    public ItemData itemType;
    
    [Tooltip("Bu ItemData türünden kaç adet eşleştirilmesi gerekiyor.")]
    public int requiredAmount;
    
    [HideInInspector]
    public int currentAmount = 0; // Seviye sırasında kaç adet toplandığı
    
    public bool IsCompleted => currentAmount >= requiredAmount;
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game Data/Level Goal")]
public class LevelGoal : ScriptableObject
{
    [Tooltip("Bu seviyedeki Item hedefleri listesi.")]
    public List<ItemGoal> itemGoals;
    
    [Tooltip("Seviye için belirlenen süre (saniye).")]
    public float timeLimit = 60f;
}