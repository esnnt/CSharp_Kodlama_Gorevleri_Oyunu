using UnityEngine;
using TMPro;

public class KodKontrol : MonoBehaviour
{
    public TMP_InputField kodAlani;
    public GorevYonetici gorevYonetici;

    public void KodCalistir()
    {
        string kod = kodAlani.text.ToLower();
        int aktifIndex = gorevYonetici.GetAktifGorevIndex();

        switch (aktifIndex)
        {
            case 0:
                // Görev 1: Console.Write ve hello world içermeli
                if (kod.Contains("console.write") && kod.Contains("hello world"))
                {
                    gorevYonetici.GorevBasarili();
                }
                break;

            case 1:
                // Görev 2: Ýki sayý ve + operatörü
                if (KodIcerikKontrol.ToplamaKontrol(kod))
                {
                    gorevYonetici.GorevBasarili();
                }
                break;


        }
    }
}