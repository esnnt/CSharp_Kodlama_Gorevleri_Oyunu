using UnityEngine;
using UnityEngine.SceneManagement;

public class closepanel : MonoBehaviour
{
    // Bu fonksiyon �a�r�ld���nda "SampleScene" adl� sahne y�klenir.
    public void QuitToSampleScene()
    {
        // SceneManager s�n�f� ile sahne ge�i�i yap�l�yor.
        SceneManager.LoadScene("SampleScene");
    }
}
