using UnityEngine;

// Sahneler arasýnda oyuncunun pozisyonunu korumak için kullanýlan yöneticidir
public class SceneTransitionManager : MonoBehaviour
{
    // Singleton örneði. Her sahnede yalnýzca bir tane olsun diye static olarak tanýmlanýr.
    public static SceneTransitionManager Instance;

    // Oyuncunun sahneye girerken nerede spawn olacaðýný tutar
    public Vector3 spawnPosition = Vector3.zero;

    void Awake()
    {
        // Eðer bu sýnýftan daha önce bir örnek oluþturulmamýþsa:
        if (Instance == null)
        {
            Instance = this;

            // Bu GameObject, sahne deðiþse bile silinmez
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Zaten bir örnek varsa yeni geleni yok et
            Destroy(gameObject);
        }
    }

    // Sahneye geçmeden önce spawn pozisyonunu dýþarýdan ayarlamak için kullanýlan metod
    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    // Spawn pozisyonunu almak için kullanýlan metod (örneðin karakter sahneye girerken bu deðeri kullanabilir)
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
