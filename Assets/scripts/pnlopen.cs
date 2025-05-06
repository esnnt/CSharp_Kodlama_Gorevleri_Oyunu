using UnityEngine;

public class pnlopen : MonoBehaviour
{
    public GameObject panel;  // Panel GameObject referansý


    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Baþlangýçta paneli kapalý yap
            Debug.Log("Panel baþlangýçta kapalý!");
        }
        else
        {
            Debug.LogWarning("Panel referansý bulunamadý!");
        }
    }

    // OpenPanel metodu, butona basýnca paneli açar
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);  // Paneli aktif yap
            Canvas.ForceUpdateCanvases();
            Debug.Log("Panel açýldý!");
        }
        else
        {
            Debug.LogError("Panel referansý bulunamadý!");
        }
    }


    // ClosePanel metodu, butona basýnca paneli kapatýr
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Paneli pasif yap
            Debug.Log("Panel kapatýldý!");
        }
        else
        {
            Debug.LogError("Panel referansý bulunamadý!");
        }
    }
}
