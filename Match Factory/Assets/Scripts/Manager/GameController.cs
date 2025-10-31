// Scripts/Presenter/GameController.cs (GÜNCELLENİYOR)

using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    [Header("Seviye Verisi")]
    [SerializeField] private LevelGoal currentLevelGoal; // Inspector'dan atanacak
    
    private float currentTime;
    private bool isGameActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Seviye başladığında çağrılır.
    public void StartLevel(LevelGoal goal)
    {
        currentLevelGoal = goal;
        currentTime = goal.timeLimit;
        isGameActive = true;
        // Tüm hedefleri sıfırla (eğer LevelGoal SO'su tekrar kullanılacaksa)
        currentLevelGoal.itemGoals.ForEach(g => g.currentAmount = 0);
        
        Debug.Log("Seviye Başladı: Hedefler Yüklendi.");
        // Spawner'ı tetikle: SimpleItemSpawner.Instance.SpawnItems(currentLevelGoal.gerekliItemler);
    }
    
    void Update()
    {
        if (!isGameActive) return;

        // 1. Zaman Takibi
        currentTime -= Time.deltaTime;
        // UIPresenter'a zamanı bildir: UIPresenter.UpdateTimer(currentTime);

        if (currentTime <= 0)
        {
            CheckGameOver(false); // Süre bitti, kaybettin
        }
    }

    /// <summary>
    /// Eşleşme başarılı olduğunda SlotManager tarafından çağrılır.
    /// </summary>
    public void OnMatchSuccess(int matchedID)
    {
        // 2. Hedef Güncelleme
        ItemGoal goalToUpdate = currentLevelGoal.itemGoals
            .FirstOrDefault(g => g.itemType.ID == matchedID);

        if (goalToUpdate != null)
        {
            goalToUpdate.currentAmount += 3; // Eşleştirme 3'lü olduğu için 3 ekle
            // UIPresenter'a hedef değişikliğini bildir.
            // UIPresenter.UpdateGoalDisplay(goalToUpdate);

            if (CheckLevelCompletion())
            {
                CheckGameOver(true); // Tüm hedefler tamamlandı, kazandın
            }
        }
    }
    
    private bool CheckLevelCompletion()
    {
        // Tüm ItemGoal'ların tamamlanıp tamamlanmadığını kontrol eder.
        return currentLevelGoal.itemGoals.All(g => g.IsCompleted);
    }

    private void CheckGameOver(bool win)
    {
        isGameActive = false;
        // Seviye sonu arayüzünü göster: UIPresenter.ShowEndGameScreen(win);
        if (win)
        {
            Debug.Log("Tebrikler, Seviye Tamamlandı!");
        }
        else
        {
            Debug.Log("Oyun Bitti. Hedeflere ulaşılamadı veya süre doldu.");
        }
    }
}