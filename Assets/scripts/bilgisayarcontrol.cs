using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bilgisayarcontrol : MonoBehaviour
{
    [Header("Bilgisayar Panel Kontrol�")]
    public GameObject bilgisayarPanel;  // Canvas2 alt�nda bulunan bilgisayar paneli (GameObject)

    [Header("Yeni Panel Kontrol�")]
    public GameObject yeniPanel;  // Yeni eklenen panel (butona bas�nca a��lacak)

    [Header("kitapcik Panel Kontrol�")]
    public GameObject kitapcikPanel;  // Yeni eklenen panel (butona bas�nca a��lacak)

   


    // Ba�lang��ta bilgisayar panelini ve yeni paneli kapat�r
    void Start()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);  // Paneli g�r�nmez yap
        }

        if (yeniPanel != null)
        {
            yeniPanel.SetActive(false);  // Yeni paneli de ba�lang��ta g�r�nmez yap
        }

        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(false);  // Yeni paneli de ba�lang��ta g�r�nmez yap
        }
    }

    // Bilgisayar panelini kapatan fonksiyon (�rne�in buton ile �a�r�l�r)
    public void Kapat()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
    }

    // Bilgisayar panelini a�an fonksiyon (�rne�in buton ile �a�r�l�r)
    public void Ac()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(true);
        }
    }
    public void EKapat()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
    }

    public void EAc()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(true);
        }
    }

    // Yeni panel'i a�an fonksiyon
    public void YeniPanelAc()
    {
        if (yeniPanel != null)
        {
            yeniPanel.SetActive(true);
        }
    }

    // Yeni panel'i kapatan fonksiyon
    public void YeniPanelKapat()
    {
        if (yeniPanel != null)
        {
            yeniPanel.SetActive(false);
        }
    }

    public void KitapcikPanelAc()
    {
        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(true);
        }
    }

    public void KitapcikPanelKapat()
    {
        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(false);
        }
    }

    // Sahnedeki t�m TMP_InputField'lar� bulur ve i�indeki yaz�y� temizler
    public void ClearAllInputs()
    {
        // Sahnedeki t�m TextMeshPro input alanlar�n� bul
        TMP_InputField[] inputs = FindObjectsOfType<TMP_InputField>();

        // Her bir input alan�n� s�f�rla
        foreach (TMP_InputField input in inputs)
        {
            if (input != null)
            {
                input.text = "";    // Text'i bo� yap
            }
        }
    }
}