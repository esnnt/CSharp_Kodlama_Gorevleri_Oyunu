using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bilgisayarcontrol : MonoBehaviour
{
    [Header("Bilgisayar Panel Kontrolü")]
    public GameObject bilgisayarPanel;  // Canvas2 altýnda bulunan bilgisayar paneli (GameObject)

    [Header("Yeni Panel Kontrolü")]
    public GameObject yeniPanel;  // Yeni eklenen panel (butona basýnca açýlacak)

    [Header("kitapcik Panel Kontrolü")]
    public GameObject kitapcikPanel;  // Yeni eklenen panel (butona basýnca açýlacak)

   


    // Baþlangýçta bilgisayar panelini ve yeni paneli kapatýr
    void Start()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);  // Paneli görünmez yap
        }

        if (yeniPanel != null)
        {
            yeniPanel.SetActive(false);  // Yeni paneli de baþlangýçta görünmez yap
        }

        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(false);  // Yeni paneli de baþlangýçta görünmez yap
        }
    }

    // Bilgisayar panelini kapatan fonksiyon (örneðin buton ile çaðrýlýr)
    public void Kapat()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
    }

    // Bilgisayar panelini açan fonksiyon (örneðin buton ile çaðrýlýr)
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

    // Yeni panel'i açan fonksiyon
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

    // Sahnedeki tüm TMP_InputField'larý bulur ve içindeki yazýyý temizler
    public void ClearAllInputs()
    {
        // Sahnedeki tüm TextMeshPro input alanlarýný bul
        TMP_InputField[] inputs = FindObjectsOfType<TMP_InputField>();

        // Her bir input alanýný sýfýrla
        foreach (TMP_InputField input in inputs)
        {
            if (input != null)
            {
                input.text = "";    // Text'i boþ yap
            }
        }
    }
}