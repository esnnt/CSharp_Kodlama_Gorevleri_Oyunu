using UnityEngine;
using TMPro;  // TextMeshPro için
using System.Diagnostics;
using System.Collections;
using JetBrains.Annotations;
using System.Text.RegularExpressions;  // Regex kullanýmý için
using UnityEngine.UI;

public class KodKontrol : MonoBehaviour
{
    [Header("Kod Kontrol Ayarlarý")]
    // Kullanýcýnýn kod yazdýðý TMP_InputField referansý
    public TMP_InputField kodAlani;
    // Görev yönetimini kontrol eden script referansý
    public GorevYonetici gorevYonetici;

    [Header("Puan Sistemi")]
    // Puan gösterilecek text objesi
    public TMP_Text puanText;
    // Her görev için verilecek puan
    public int gorevPuani = 20;

    [Header("Kahve Satýn Alma Sistemi")]
    // Kahve sayýsýný gösteren text (satýn alýnan kahve)
    public TMP_Text kahveText;
    // Kahve fiyatý
    public int kahveFiyati = 10;

    [Header("Envanter Sistemi")]
    // Envanter paneli (açýlýp kapanabilir)
    public GameObject envanterPaneli;
    // Kahve objesi (kahve varsa görünür, yoksa gizli)
    public GameObject kahveObjesi;
    // Kahve içme butonu
    public Button kahveIcButonu;
    // Kahve görseli (opsiyonel)
    public Image kahveImage;

    [Header("Kahve Efektleri")]
    // Kahve içildiðinde verilecek bonuslar
    public int kahveEnerjiBonus = 5;
    // Enerji bar referansý - kahve içildiðinde enerjiyi artýrmak için
    public energybar enerjiBarScript;
    // Not: Kahve puan vermez, sadece enerji/saðlýk verir

    // Toplam puan (PlayerPrefs ile kaydedilir)
    private int toplamPuan;
    // Kahve sayýsý (PlayerPrefs ile kaydedilir - hem satýn alma hem envanter için)
    private int kahveSayisi;

    private void Start()
    {
        // Oyun baþladýðýnda kaydedilmiþ deðerleri yükle
        toplamPuan = PlayerPrefs.GetInt("ToplamPuan", 0);
        kahveSayisi = PlayerPrefs.GetInt("KahveSayisi", 0);

        PuanTextGuncelle();
        KahveTextGuncelle();
        KahveObjeVisibilityGuncelle();

        // Kahve iç butonuna listener ekle
        if (kahveIcButonu != null)
        {
            kahveIcButonu.onClick.AddListener(KahveIc);
        }

        // Envanter paneli baþlangýçta kapalý olsun
        if (envanterPaneli != null)
        {
            envanterPaneli.SetActive(false);
        }
    }

    // Kullanýcýnýn yazdýðý kodun toplama iþlemi içerip içermediðini kontrol eder.
    public static bool ToplamaKontrol(string kod)
    {
        // Kodda '+' iþaretinin olup olmadýðý kontrol edilir.
        bool artiVar = kod.Contains("+");
        // Kodda en az iki sayý olup olmadýðý kontrol edilir.
        var sayilar = Regex.Matches(kod, @"\d+");
        bool enAzIkiSayiVar = sayilar.Count >= 2;
        // '+' iþareti var ve en az iki sayý varsa true döner.
        return artiVar && enAzIkiSayiVar;
    }

    // Puan ekleme fonksiyonu
    private void PuanEkle(int puan)
    {
        toplamPuan += puan;
        // Puaný PlayerPrefs ile kaydet (oyun kapanýnca kaybolmasýn)
        PlayerPrefs.SetInt("ToplamPuan", toplamPuan);
        PlayerPrefs.Save();
        // Text'i güncelle
        PuanTextGuncelle();

        // Puan kazanma efekti (opsiyonel)
        StartCoroutine(PuanKazanmaEfekti());
    }

    // Puan text'ini güncelleme fonksiyonu
    private void PuanTextGuncelle()
    {
        if (puanText != null)
        {
            puanText.text = toplamPuan.ToString();
        }
    }

    // Kahve text'ini güncelleme fonksiyonu
    private void KahveTextGuncelle()
    {
        if (kahveText != null)
        {
            kahveText.text = kahveSayisi.ToString();
        }
    }

    // Kahve objesinin görünürlüðünü güncelle (kahve varsa göster, yoksa gizle)
    private void KahveObjeVisibilityGuncelle()
    {
        if (kahveObjesi != null)
        {
            kahveObjesi.SetActive(kahveSayisi > 0);
        }

        // Kahve iç butonu da kahve varsa aktif
        if (kahveIcButonu != null)
        {
            kahveIcButonu.interactable = kahveSayisi > 0;
        }
    }

    // Kahve satýn alma fonksiyonu (10P butonuna basýldýðýnda çaðrýlýr)
    public void KahveSatinAl()
    {
        // Yeterli puan var mý kontrol et
        if (toplamPuan >= kahveFiyati)
        {
            // Puaný düþ
            toplamPuan -= kahveFiyati;
            // Kahve sayýsýný arttýr
            kahveSayisi++;

            // Deðiþiklikleri kaydet
            PlayerPrefs.SetInt("ToplamPuan", toplamPuan);
            PlayerPrefs.SetInt("KahveSayisi", kahveSayisi);
            PlayerPrefs.Save();

            // Text'leri güncelle
            PuanTextGuncelle();
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();

            // Kahve satýn alma efekti
            StartCoroutine(KahveSatinAlmaEfekti());
        }
        else
        {
            // Yeterli puan yok uyarýsý
            StartCoroutine(YetersizPuanUyarisi());
        }
    }

    // Kahve içme fonksiyonu
    public void KahveIc()
    {
        if (kahveSayisi > 0)
        {
            // Kahve sayýsýný azalt
            kahveSayisi--;

            // Kahve içildiðinde enerjiyi %50 artýr
            if (enerjiBarScript != null)
            {
                float enerjiArtisi = enerjiBarScript.maxEnerji * 0.5f; // %50 hesapla
                enerjiBarScript.mevcutEnerji += enerjiArtisi;

                // Enerji maximum deðerini aþmasýn
                if (enerjiBarScript.mevcutEnerji > enerjiBarScript.maxEnerji)
                {
                    enerjiBarScript.mevcutEnerji = enerjiBarScript.maxEnerji;
                }

                // Enerji deðerini kaydet
                PlayerPrefs.SetFloat("mevcutEnerji", enerjiBarScript.mevcutEnerji);
                PlayerPrefs.Save();

                // Karartma kontrolünü yap (enerji yükseldiyse karartmayý temizle)
                enerjiBarScript.KarartmaKontrolEt();
            }

            // Deðiþiklikleri kaydet
            PlayerPrefs.SetInt("KahveSayisi", kahveSayisi);
            PlayerPrefs.Save();

            // Text'leri ve görünürlüðü güncelle
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();

            // Kahve içme efekti
            StartCoroutine(KahveIcmeEfekti());
        }
    }

    // Envanter panelini aç/kapat
    public void EnvanterPaneliToggle()
    {
        if (envanterPaneli != null)
        {
            envanterPaneli.SetActive(!envanterPaneli.activeInHierarchy);
        }
    }

    // Kahve satýn alma efekti
    private IEnumerator KahveSatinAlmaEfekti()
    {
        if (kahveText != null)
        {
            Vector3 originalScale = kahveText.transform.localScale;
            Color originalColor = kahveText.color;

            // Text'i yeþil yap ve büyüt
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

    // Kahve transfer efekti (artýk kullanýlmýyor ama silmeyeyim)
    private IEnumerator KahveTransferEfekti()
    {
        // Bu fonksiyon artýk kullanýlmýyor ama compatibility için býrakýyorum
        yield return null;
    }

    // Kahve içme efekti
    private IEnumerator KahveIcmeEfekti()
    {
        if (kahveImage != null)
        {
            Color originalColor = kahveImage.color;

            // Kahve görselini sarý yap (enerji efekti)
            kahveImage.color = Color.yellow;
            yield return new WaitForSeconds(0.2f);

            kahveImage.color = originalColor;
        }

        // Puan artýþ efekti de çalýþtýr (artýk çalýþmýyor çünkü puan vermiyor)
        // StartCoroutine(PuanKazanmaEfekti());
    }

    // Yetersiz puan uyarýsý efekti
    private IEnumerator YetersizPuanUyarisi()
    {
        if (puanText != null)
        {
            Color originalColor = puanText.color;

            // Text'i kýrmýzý yap ve titret
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

            // Text'i büyüt
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

    // Kullanýcýnýn yazdýðý kodu görev kriterlerine göre deðerlendirir ve eðer doðruysa görevi baþarýlý sayar.
    public void KodCalistir()
    {
        // Kullanýcýnýn yazdýðý kodu al, küçük harfe çevir (büyük-küçük harf farkýný önlemek için)
        string kod = kodAlani.text.ToLower();
        // Aktif görev indeksini al (görev yöneticisinden)
        int aktifIndex = gorevYonetici.GetAktifGorevIndex();

        // Aktif göreve göre kodu kontrol et
        switch (aktifIndex)
        {
            case 0:
                // Görev 0 için koþul:
                // Kodda "console.write" ve "hello world" ifadeleri bulunmalý
                if (kod.Contains("console.write") && kod.Contains("hello world"))
                {
                    // Koþullar saðlanýrsa görevi baþarýlý say ve puan ekle
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;

            case 1:
                // Görev 1 için koþul:
                // Toplama iþlemi kontrolü (örneðin "5 + 3" gibi)
                if (ToplamaKontrol(kod))
                {
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;

            case 2:
                // Görev 2 için koþullar:
                // Kodda "int" ve "sayi" kelimeleri olmalý
                bool sayiVar = kod.Contains("int") && kod.Contains("sayi");
                // "if" ve "else" ifadeleri olmalý
                bool ifVar = kod.Contains("if");
                bool elseVar = kod.Contains("else");
                // Karþýlaþtýrma için '>' ve '10' rakamý olmalý
                bool buyuktur10 = kod.Contains(">") && kod.Contains("10");
                // Konsola yazdýrma iþlemi için "console.write" olmalý
                bool consoleVar = kod.Contains("console.write");
                // Yukarýdaki tüm koþullar saðlanýyorsa görevi baþarýlý say ve puan ekle
                if (sayiVar && ifVar && elseVar && buyuktur10 && consoleVar)
                {
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                }
                break;
        }
    }

    // Puan sýfýrlama fonksiyonu (test için veya oyunu yeniden baþlatmak için)
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

    // Toplam puaný döndüren fonksiyon (baþka scriptlerden kullanýlabilir)
    public int GetToplamPuan()
    {
        return toplamPuan;
    }

    // Kahve sayýsýný döndüren fonksiyon (baþka scriptlerden kullanýlabilir)
    public int GetKahveSayisi()
    {
        return kahveSayisi;
    }
}