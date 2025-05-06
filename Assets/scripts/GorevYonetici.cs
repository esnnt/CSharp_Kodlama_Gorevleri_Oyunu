using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GorevYonetici : MonoBehaviour
{
    public TMP_Text gorevText;
    public List<string> gorevListesi;
    private int aktifGorevIndex = 0;

    void Start()
    {
        GoreviGoster();
    }

    public void GorevBasarili()
    {
        gorevText.text = "Görev Baþarýlý!";
        gorevText.color = Color.green;
        StartCoroutine(SonrakiGoreveGec());
    }

    IEnumerator SonrakiGoreveGec()
    {
        yield return new WaitForSeconds(4f);

        aktifGorevIndex++;

        if (aktifGorevIndex < gorevListesi.Count)
        {
            GoreviGoster();
        }
        else
        {
            gorevText.text = "Tüm görevler tamamlandý!";
        }
    }

    public void GoreviGoster()
    {
        gorevText.text = gorevListesi[aktifGorevIndex];
        gorevText.color = Color.white;
    }

    public int GetAktifGorevIndex()
    {
        return aktifGorevIndex;
    }
}
