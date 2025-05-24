using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bilgisayarcontrol : MonoBehaviour
{
    [Header("Bilgisayar Panel Kontrolü")]
    public GameObject bilgisayarPanel;  // Canvas2 altýndaki Panel objesi
    
    void Start()
    {
        // Panel baþlangýçta kapalý olsun
        if (bilgisayarPanel != null)
            bilgisayarPanel.SetActive(false);
    }
    
    public void Kapat()
    {
        if (bilgisayarPanel != null)
            bilgisayarPanel.SetActive(false);
    }
    
    public void Ac()
    {
        if (bilgisayarPanel != null)
            bilgisayarPanel.SetActive(true);
    }
    
    public void ClearAllInputs()
    {
        TMP_InputField[] inputs = FindObjectsOfType<TMP_InputField>();
        foreach (TMP_InputField input in inputs)
        {
            if (input != null)
                input.text = "";
        }
    }
}