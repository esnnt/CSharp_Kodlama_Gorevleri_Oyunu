using UnityEngine;

public class pnlopen : MonoBehaviour
{
    public GameObject panel;  // Panel GameObject referans�


    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Ba�lang��ta paneli kapal� yap
            Debug.Log("Panel ba�lang��ta kapal�!");
        }
        else
        {
            Debug.LogWarning("Panel referans� bulunamad�!");
        }
    }

    // OpenPanel metodu, butona bas�nca paneli a�ar
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);  // Paneli aktif yap
            Canvas.ForceUpdateCanvases();
            Debug.Log("Panel a��ld�!");
        }
        else
        {
            Debug.LogError("Panel referans� bulunamad�!");
        }
    }


    // ClosePanel metodu, butona bas�nca paneli kapat�r
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);  // Paneli pasif yap
            Debug.Log("Panel kapat�ld�!");
        }
        else
        {
            Debug.LogError("Panel referans� bulunamad�!");
        }
    }
}
