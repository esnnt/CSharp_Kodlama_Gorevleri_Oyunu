using UnityEngine;
using TMPro;  // TextMeshPro i�in
using System.Diagnostics;
using System.Collections;
using JetBrains.Annotations;
using System.Text.RegularExpressions;  // Regex kullan�m� i�in
using UnityEngine.UI;

public class KodKontrol : MonoBehaviour
{
    [Header("Kod Kontrol Ayarlar�")]
    // Kullan�c�n�n kod yazd��� TMP_InputField referans�
    public TMP_InputField kodAlani;
    // G�rev y�netimini kontrol eden script referans�
    public GorevYonetici gorevYonetici;

    [Header("Puan Sistemi")]
    // Puan g�sterilecek text objesi
    public TMP_Text puanText;
    // Her g�rev i�in verilecek puan
    public int gorevPuani = 20;

    [Header("Kahve Sat�n Alma Sistemi")]
    // Kahve say�s�n� g�steren text (sat�n al�nan kahve)
    public TMP_Text kahveText;
    // Kahve fiyat�
    public int kahveFiyati = 10;

    [Header("Envanter Sistemi")]
    // Envanter paneli (a��l�p kapanabilir)
    public GameObject envanterPaneli;
    // Kahve objesi (kahve varsa g�r�n�r, yoksa gizli)
    public GameObject kahveObjesi;
    // Kahve i�me butonu
    public Button kahveIcButonu;
    // Kahve g�rseli (opsiyonel)
    public Image kahveImage;

    [Header("Kahve Efektleri")]
    // Kahve i�ildi�inde verilecek bonuslar
    public int kahveEnerjiBonus = 5;
    // Enerji bar referans� - kahve i�ildi�inde enerjiyi art�rmak i�in
    public energybar enerjiBarScript;
    // Not: Kahve puan vermez, sadece enerji/sa�l�k verir

    // Toplam puan (PlayerPrefs ile kaydedilir)
    private int toplamPuan;
    // Kahve say�s� (PlayerPrefs ile kaydedilir - hem sat�n alma hem envanter i�in)
    private int kahveSayisi;

    private void Start()
    {
        // Oyun ba�lad���nda kaydedilmi� de�erleri y�kle
        toplamPuan = PlayerPrefs.GetInt("ToplamPuan", 0);
        kahveSayisi = PlayerPrefs.GetInt("KahveSayisi", 0);

        PuanTextGuncelle();
        KahveTextGuncelle();
        KahveObjeVisibilityGuncelle();

        // Kahve i� butonuna listener ekle
        if (kahveIcButonu != null)
        {
            kahveIcButonu.onClick.AddListener(KahveIc);
        }

        // Envanter paneli ba�lang��ta kapal� olsun
        if (envanterPaneli != null)
        {
            envanterPaneli.SetActive(false);
        }
    }

    // Kullan�c�n�n yazd��� kodun toplama i�lemi i�erip i�ermedi�ini kontrol eder.
    public static bool ToplamaKontrol(string kod)
    {
        // Kodda '+' i�aretinin olup olmad��� kontrol edilir.
        bool artiVar = kod.Contains("+");
        // Kodda en az iki say� olup olmad��� kontrol edilir.
        var sayilar = Regex.Matches(kod, @"\d+");
        bool enAzIkiSayiVar = sayilar.Count >= 2;
        // '+' i�areti var ve en az iki say� varsa true d�ner.
        return artiVar && enAzIkiSayiVar;
    }

    // Puan ekleme fonksiyonu
    private void PuanEkle(int puan)
    {
        toplamPuan += puan;
        // Puan� PlayerPrefs ile kaydet (oyun kapan�nca kaybolmas�n)
        PlayerPrefs.SetInt("ToplamPuan", toplamPuan);
        PlayerPrefs.Save();
        // Text'i g�ncelle
        PuanTextGuncelle();

        // Puan kazanma efekti (opsiyonel)
        StartCoroutine(PuanKazanmaEfekti());
    }

    // Puan text'ini g�ncelleme fonksiyonu
    private void PuanTextGuncelle()
    {
        if (puanText != null)
        {
            puanText.text = toplamPuan.ToString();
        }
    }

    // Kahve text'ini g�ncelleme fonksiyonu
    private void KahveTextGuncelle()
    {
        if (kahveText != null)
        {
            kahveText.text = kahveSayisi.ToString();
        }
    }

    // Kahve objesinin g�r�n�rl���n� g�ncelle (kahve varsa g�ster, yoksa gizle)
    private void KahveObjeVisibilityGuncelle()
    {
        if (kahveObjesi != null)
        {
            kahveObjesi.SetActive(kahveSayisi > 0);
        }

        // Kahve i� butonu da kahve varsa aktif
        if (kahveIcButonu != null)
        {
            kahveIcButonu.interactable = kahveSayisi > 0;
        }
    }

    // Kahve sat�n alma fonksiyonu (10P butonuna bas�ld���nda �a�r�l�r)
    public void KahveSatinAl()
    {
        // Yeterli puan var m� kontrol et
        if (toplamPuan >= kahveFiyati)
        {
            // Puan� d��
            toplamPuan -= kahveFiyati;
            // Kahve say�s�n� artt�r
            kahveSayisi++;

            // De�i�iklikleri kaydet
            PlayerPrefs.SetInt("ToplamPuan", toplamPuan);
            PlayerPrefs.SetInt("KahveSayisi", kahveSayisi);
            PlayerPrefs.Save();

            // Text'leri g�ncelle
            PuanTextGuncelle();
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();

            // Kahve sat�n alma efekti
            StartCoroutine(KahveSatinAlmaEfekti());
        }
        else
        {
            // Yeterli puan yok uyar�s�
            StartCoroutine(YetersizPuanUyarisi());
        }
    }

    // Kahve i�me fonksiyonu
    public void KahveIc()
    {
        if (kahveSayisi > 0)
        {
            // Kahve say�s�n� azalt
            kahveSayisi--;

            // Kahve i�ildi�inde enerjiyi %50 art�r
            if (enerjiBarScript != null)
            {
                float enerjiArtisi = enerjiBarScript.maxEnerji * 0.5f; // %50 hesapla
                enerjiBarScript.mevcutEnerji += enerjiArtisi;

                // Enerji maximum de�erini a�mas�n
                if (enerjiBarScript.mevcutEnerji > enerjiBarScript.maxEnerji)
                {
                    enerjiBarScript.mevcutEnerji = enerjiBarScript.maxEnerji;
                }

                // Enerji de�erini kaydet
                PlayerPrefs.SetFloat("mevcutEnerji", enerjiBarScript.mevcutEnerji);
                PlayerPrefs.Save();

                // Karartma kontrol�n� yap (enerji y�kseldiyse karartmay� temizle)
                enerjiBarScript.KarartmaKontrolEt();
            }

            // De�i�iklikleri kaydet
            PlayerPrefs.SetInt("KahveSayisi", kahveSayisi);
            PlayerPrefs.Save();

            // Text'leri ve g�r�n�rl��� g�ncelle
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();

            // Kahve i�me efekti
            StartCoroutine(KahveIcmeEfekti());
        }
    }

    // Envanter panelini a�/kapat
    public void EnvanterPaneliToggle()
    {
        if (envanterPaneli != null)
        {
            envanterPaneli.SetActive(!envanterPaneli.activeInHierarchy);
        }
    }

    // Kahve sat�n alma efekti
    private IEnumerator KahveSatinAlmaEfekti()
    {
        if (kahveText != null)
        {
            Vector3 originalScale = kahveText.transform.localScale;
            Color originalColor = kahveText.color;

            // Text'i ye�il yap ve b�y�t
            kahveText.color = Color.green;

            float duration = 0.3f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float scale = Mathf.Lerp(1f, 1.5f, elapsed / duration);
                kahveText.transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Normal boyuta geri getir
            elapsed = 0f;
            while (elapsed < duration)
            {
                float scale = Mathf.Lerp(1.5f, 1f, elapsed / duration);
                kahveText.transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            kahveText.transform.localScale = originalScale;
            kahveText.color = originalColor;
        }
    }

    // Kahve transfer efekti (art�k kullan�lm�yor ama silmeyeyim)
    private IEnumerator KahveTransferEfekti()
    {
        // Bu fonksiyon art�k kullan�lm�yor ama compatibility i�in b�rak�yorum
        yield return null;
    }

    // Kahve i�me efekti
    private IEnumerator KahveIcmeEfekti()
    {
        if (kahveImage != null)
        {
            Color originalColor = kahveImage.color;

            // Kahve g�rselini sar� yap (enerji efekti)
            kahveImage.color = Color.yellow;
            yield return new WaitForSeconds(0.2f);

            kahveImage.color = originalColor;
        }

        // Puan art�� efekti de �al��t�r (art�k �al��m�yor ��nk� puan vermiyor)
        // StartCoroutine(PuanKazanmaEfekti());
    }

    // Yetersiz puan uyar�s� efekti
    private IEnumerator YetersizPuanUyarisi()
    {
        if (puanText != null)
        {
            Color originalColor = puanText.color;

            // Text'i k�rm�z� yap ve titret
            for (int i = 0; i < 3; i++)
            {
                puanText.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                puanText.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator PuanKazanmaEfekti()
    {
        if (puanText != null)
        {
            Vector3 originalScale = puanText.transform.localScale;

            // Text'i b�y�t
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float scale = Mathf.Lerp(1f, 1.3f, elapsed / duration);
                puanText.transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Text'i normal boyutuna geri getir
            elapsed = 0f;
            while (elapsed < duration)
            {
                float scale = Mathf.Lerp(1.3f, 1f, elapsed / duration);
                puanText.transform.localScale = originalScale * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            puanText.transform.localScale = originalScale;
        }
    }

    // Kullan�c�n�n yazd��� kodu g�rev kriterlerine g�re de�erlendirir ve e�er do�ruysa g�revi ba�ar�l� sayar.
    public void KodCalistir()
    {
        // Kullan�c�n�n yazd��� kodu al, k���k harfe �evir (b�y�k-k���k harf fark�n� �nlemek i�in)
        string kod = kodAlani.text.ToLower();
        // Aktif g�rev indeksini al (g�rev y�neticisinden)
        int aktifIndex = gorevYonetici.GetAktifGorevIndex();

        // Aktif g�reve g�re kodu kontrol et
        switch (aktifIndex)
        {
            case 0:
                // G�rev 0 i�in ko�ul:
                // Kodda "console.write" ve "hello world" ifadeleri bulunmal�
                if (kod.Contains("console.write") && kod.Contains("hello world"))
                {
                    // Ko�ullar sa�lan�rsa g�revi ba�ar�l� say ve puan ekle
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;

            case 1:
                // G�rev 1 i�in ko�ul:
                // Toplama i�lemi kontrol� (�rne�in "5 + 3" gibi)
                if (ToplamaKontrol(kod))
                {
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;

            case 2:
                // G�rev 2 i�in ko�ullar:
                // Kodda "int" ve "sayi" kelimeleri olmal�
                bool sayiVar = kod.Contains("int") && kod.Contains("sayi");
                // "if" ve "else" ifadeleri olmal�
                bool ifVar = kod.Contains("if");
                bool elseVar = kod.Contains("else");
                // Kar��la�t�rma i�in '>' ve '10' rakam� olmal�
                bool buyuktur10 = kod.Contains(">") && kod.Contains("10");
                // Konsola yazd�rma i�lemi i�in "console.write" olmal�
                bool consoleVar = kod.Contains("console.write");
                // Yukar�daki t�m ko�ullar sa�lan�yorsa g�revi ba�ar�l� say ve puan ekle
                if (sayiVar && ifVar && elseVar && buyuktur10 && consoleVar)
                {
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;
        }
    }

    // Puan s�f�rlama fonksiyonu (test i�in veya oyunu yeniden ba�latmak i�in)
    public void PuanSifirla()
    {
        toplamPuan = 0;
        kahveSayisi = 0;
        PlayerPrefs.SetInt("ToplamPuan", 0);
        PlayerPrefs.SetInt("KahveSayisi", 0);
        PlayerPrefs.Save();
        PuanTextGuncelle();
        KahveTextGuncelle();
        KahveObjeVisibilityGuncelle();
    }

    // Toplam puan� d�nd�ren fonksiyon (ba�ka scriptlerden kullan�labilir)
    public int GetToplamPuan()
    {
        return toplamPuan;
    }

    // Kahve say�s�n� d�nd�ren fonksiyon (ba�ka scriptlerden kullan�labilir)
    public int GetKahveSayisi()
    {
        return kahveSayisi;
    }
}