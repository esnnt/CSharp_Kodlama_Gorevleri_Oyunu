using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public Vector3 spawnPosition = Vector3.zero;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþse bile yok olma
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
