using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Collections;
using JetBrains.Annotations;

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
                if (kod.Contains("console.write") && kod.Contains("hello world"))
                {
                    gorevYonetici.GorevBasarili();
                }
                
                    break;
                

            case 1:
                if (KodIcerikKontrol.ToplamaKontrol(kod))
                {
                    gorevYonetici.GorevBasarili();
                }
                break;

            case 2:
                bool sayiVar = kod.Contains("int") && kod.Contains("sayi");
                bool ifVar = kod.Contains("if");
                bool elseVar = kod.Contains("else");
                bool buyuktur10 = kod.Contains(">") && kod.Contains("10");
                bool consoleVar = kod.Contains("console.write");

                if (sayiVar && ifVar && elseVar && buyuktur10 && consoleVar)
                {
                    gorevYonetici.GorevBasarili();
                }
                break;
        }
    }
}
