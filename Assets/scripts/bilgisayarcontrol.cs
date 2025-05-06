using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bilgisayarcontrol : MonoBehaviour
{
    public GameObject bilgisayarPanel;  // Canvas2 altýndaki Panel objesi

    public void Kapat()
    {
        bilgisayarPanel.SetActive(false);
    }

    public void Ac()
    {
        bilgisayarPanel.SetActive(true);
    }
    public void ClearAllInputs()
    {
        TMP_InputField[] inputs = FindObjectsOfType<TMP_InputField>();

        foreach (TMP_InputField input in inputs)
        {
            input.text = "";
        }
    }
}
