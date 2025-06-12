using UnityEngine;

public class pnlopen : MonoBehaviour
{
    public GameObject panel;  // Panel GameObject referans�, Inspector'dan atanacak

    // Oyun ba�lad���nda �al���r
    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Ba�lang��ta paneli kapat
            Debug.Log("Panel ba�lang��ta kapal�!");  // Konsola bilgi mesaj� yaz
        }
        else
        {
            Debug.LogWarning("Panel referans� bulunamad�!");  // Panel atanmad�ysa uyar� ver
        }
    }

    // Bu fonksiyon bir butona ba�lanabilir, paneli a�ar
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);   // Paneli g�r�n�r yap
            Canvas.ForceUpdateCanvases();  // Canvas g�ncellemesini zorla (UI g�ncelleme i�in)
            Debug.Log("Panel a��ld�!");  // Konsola bilgi mesaj� yaz
        }
        else
        {
            Debug.LogError("Panel referans� bulunamad�!");  // Panel referans� yoksa hata mesaj�
        }
    }

    // Bu fonksiyon bir butona ba�lanabilir, paneli kapat�r
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Paneli gizle
            Debug.Log("Panel kapat�ld�!");  // Konsola bilgi mesaj� yaz
        }
        else
        {
            Debug.LogError("Panel referans� bulunamad�!");  // Panel referans� yoksa hata mesaj�
        }
    }
}
