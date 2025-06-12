using UnityEngine;

public class pnlopen : MonoBehaviour
{
    public GameObject panel;  // Panel GameObject referansý, Inspector'dan atanacak

    // Oyun baþladýðýnda çalýþýr
    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Baþlangýçta paneli kapat
            Debug.Log("Panel baþlangýçta kapalý!");  // Konsola bilgi mesajý yaz
        }
        else
        {
            Debug.LogWarning("Panel referansý bulunamadý!");  // Panel atanmadýysa uyarý ver
        }
    }

    // Bu fonksiyon bir butona baðlanabilir, paneli açar
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);   // Paneli görünür yap
            Canvas.ForceUpdateCanvases();  // Canvas güncellemesini zorla (UI güncelleme için)
            Debug.Log("Panel açýldý!");  // Konsola bilgi mesajý yaz
        }
        else
        {
            Debug.LogError("Panel referansý bulunamadý!");  // Panel referansý yoksa hata mesajý
        }
    }

    // Bu fonksiyon bir butona baðlanabilir, paneli kapatýr
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Paneli gizle
            Debug.Log("Panel kapatýldý!");  // Konsola bilgi mesajý yaz
        }
        else
        {
            Debug.LogError("Panel referansý bulunamadý!");  // Panel referansý yoksa hata mesajý
        }
    }
}
