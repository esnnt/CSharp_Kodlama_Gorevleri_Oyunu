using UnityEngine;

// Sahneler aras�nda oyuncunun pozisyonunu korumak i�in kullan�lan y�neticidir
public class SceneTransitionManager : MonoBehaviour
{
    // Singleton �rne�i. Her sahnede yaln�zca bir tane olsun diye static olarak tan�mlan�r.
    public static SceneTransitionManager Instance;

    // Oyuncunun sahneye girerken nerede spawn olaca��n� tutar
    public Vector3 spawnPosition = Vector3.zero;

    void Awake()
    {
        // E�er bu s�n�ftan daha �nce bir �rnek olu�turulmam��sa:
        if (Instance == null)
        {
            Instance = this;

            // Bu GameObject, sahne de�i�se bile silinmez
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Zaten bir �rnek varsa yeni geleni yok et
            Destroy(gameObject);
        }
    }

    // Sahneye ge�meden �nce spawn pozisyonunu d��ar�dan ayarlamak i�in kullan�lan metod
    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    // Spawn pozisyonunu almak i�in kullan�lan metod (�rne�in karakter sahneye girerken bu de�eri kullanabilir)
    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
