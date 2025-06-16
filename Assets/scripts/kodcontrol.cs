using UnityEngine;
using TMPro;  // TextMeshPro için
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

    [Header("cookie Satýn Alma Sistemi")]
    // Kahve sayýsýný gösteren text (satýn alýnan kahve)
    public TMP_Text cookietext;
    // Kahve fiyatý
    public int cookiefiyati = 5;

    [Header("Envanter Sistemi")]
    // Envanter paneli (açýlýp kapanabilir)
    public GameObject envanterPaneli;
    // Kahve objesi (kahve varsa görünür, yoksa gizli)
    public GameObject kahveObjesi;

    public GameObject cookieObjesi;
    // Kahve içme butonu
    public Button kahveIcButonu;
    public Button cookieyeButonu;
    // Kahve görseli (opsiyonel)
    public Image kahveImage;
    public Image cookieImage;


    [Header("Kahve Efektleri")]
    // Kahve içildiðinde verilecek bonuslar
    public int kahveEnerjiBonus = 5;
    // Enerji bar referansý - kahve içildiðinde enerjiyi artýrmak için
    public energybar enerjiBarScript;
    // Not: Kahve puan vermez, sadece enerji/saðlýk verir


    [Header("cookie Efektleri")]
    // Kahve içildiðinde verilecek bonuslar
    public int cookieEnerjiBonus = 2;
    // Enerji bar referansý - kahve içildiðinde enerjiyi artýrmak için
    public energybar enerjiBarScripti;
    // Not: Kahve puan vermez, sadece enerji/saðlýk verir

    [Header("Puan Kazanma Mesaj Sistemi")]
    // Puan kazanýldýðýnda gösterilecek ikinci mesaj (Inspector'dan atanacak)
    public GameObject imageMessage2;

    [Header("Satýn Alma Mesaj Sistemi")]
    // Satýn alma yapýldýðýnda gösterilecek üçüncü mesaj (Inspector'dan atanacak)
    public GameObject imageMessage3;

    [Header("Puan Mesajý Animasyon Ayarlarý")]
    // Mesajýn animasyon süreleri
    public float puanFadeInSuresi = 1f;        // Mesajýn gelirken geçeceði süre
    public float puanGoruntulenmeSuresi = 7f;  // Mesajýn tam görünür kalacaðý süre
    public float puanFadeOutSuresi = 2f;       // Mesajýn kaybolurken geçeceði süre
    public float puanHareketMesafesi = 100f;   // Aþaðýdan yukarý hareket mesafesi

    [Header("Satýn Alma Mesajý Animasyon Ayarlarý")]
    // Satýn alma mesajýnýn animasyon süreleri
    public float satinAlmaFadeInSuresi = 1f;        // Mesajýn gelirken geçeceði süre
    public float satinAlmaGoruntulenmeSuresi = 5f;  // Mesajýn tam görünür kalacaðý süre
    public float satinAlmaFadeOutSuresi = 1.5f;     // Mesajýn kaybolurken geçeceði süre
    public float satinAlmaHareketMesafesi = 80f;    // Aþaðýdan yukarý hareket mesafesi

    // Puan mesajý animasyon durumlarý
    private bool puanMesajiGosteriliyorMu = false;
    private Coroutine puanMesajiAnimasyonCoroutine;

    // Satýn alma mesajý animasyon durumlarý
    private bool satinAlmaMesajiGosteriliyorMu = false;
    private Coroutine satinAlmaMesajiAnimasyonCoroutine;

    // Toplam puan (PlayerPrefs ile kaydedilir)
    private int toplamPuan;
    // Kahve sayýsý (PlayerPrefs ile kaydedilir - hem satýn alma hem envanter için)
    private int kahveSayisi;
    private int cookiesayisi;

    private void Start()
    {
        Debug.Log("Start metodunda hataMesajiText: " + hataMesajiText);
        // Oyun baþladýðýnda kaydedilmiþ deðerleri yükle
        toplamPuan = PlayerPrefs.GetInt("ToplamPuan", 0);
        kahveSayisi = PlayerPrefs.GetInt("KahveSayisi", 0);
        cookiesayisi = PlayerPrefs.GetInt("cookiesayisi", 0);

        PuanTextGuncelle();
        KahveTextGuncelle();
        cookieTextGuncelle();

        KahveObjeVisibilityGuncelle();
        cookieObjeVisibilityGuncelle();

        // Kahve iç butonuna listener ekle
        if (kahveIcButonu != null)
        {
            kahveIcButonu.onClick.AddListener(KahveIc);
        }
        if (cookieyeButonu != null)
        {
            cookieyeButonu.onClick.AddListener(cookieye);
        }

        // Envanter paneli baþlangýçta kapalý olsun
        if (envanterPaneli != null)
        {
            envanterPaneli.SetActive(false);
        }

        // Mesaj objelerini baþlangýçta gizle
        if (imageMessage2 != null)
        {
            imageMessage2.SetActive(false);
        }
        if (imageMessage3 != null)
        {
            imageMessage3.SetActive(false);
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

        // Puan kazanýldýðýnda imageMessage2'yi göster
        PuanKazanmaMesajiGoster();
    }

    // Puan kazanýldýðýnda imageMessage2'yi gösteren fonksiyon
    private void PuanKazanmaMesajiGoster()
    {
        if (imageMessage2 != null && !puanMesajiGosteriliyorMu)
        {
            // Eðer zaten bir animasyon coroutine çalýþýyorsa durdur
            if (puanMesajiAnimasyonCoroutine != null)
            {
                StopCoroutine(puanMesajiAnimasyonCoroutine);
            }

            // Puan mesajý animasyonunu baþlat
            puanMesajiAnimasyonCoroutine = StartCoroutine(PuanMesajiAnimasyonu());
        }
    }

    // Satýn alma yapýldýðýnda imageMessage3'ü gösteren fonksiyon
    private void SatinAlmaMesajiGoster()
    {
        if (imageMessage3 != null && !satinAlmaMesajiGosteriliyorMu)
        {
            // Eðer zaten bir animasyon coroutine çalýþýyorsa durdur
            if (satinAlmaMesajiAnimasyonCoroutine != null)
            {
                StopCoroutine(satinAlmaMesajiAnimasyonCoroutine);
            }

            // Satýn alma mesajý animasyonunu baþlat
            satinAlmaMesajiAnimasyonCoroutine = StartCoroutine(SatinAlmaMesajiAnimasyonu());
        }
    }

    // Puan mesajý animasyonunu yöneten coroutine
    IEnumerator PuanMesajiAnimasyonu()
    {
        puanMesajiGosteriliyorMu = true;

        // Mesajý aktif et
        imageMessage2.SetActive(true);

        // Mesajýn RectTransform ve CanvasGroup bileþenlerini al
        RectTransform rectTransform = imageMessage2.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = imageMessage2.GetComponent<CanvasGroup>();

        // Eðer CanvasGroup yoksa ekle
        if (canvasGroup == null)
        {
            canvasGroup = imageMessage2.AddComponent<CanvasGroup>();
        }

        // Baþlangýç pozisyonunu kaydet
        Vector3 baslangicPozisyon = rectTransform.anchoredPosition;
        Vector3 animasyonBaslangicPozisyon = new Vector3(baslangicPozisyon.x, baslangicPozisyon.y - puanHareketMesafesi, baslangicPozisyon.z);

        // Baþlangýçta görünmez ve aþaðýda konumlandýr
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = animasyonBaslangicPozisyon;

        // FADE IN ve YUKARI HAREKET ANÝMASYONU
        float elapsedTime = 0f;
        while (elapsedTime < puanFadeInSuresi)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / puanFadeInSuresi;

            // Smooth geçiþ için easing
            t = Mathf.SmoothStep(0f, 1f, t);

            // Alpha ve pozisyon interpolasyonu
            canvasGroup.alpha = t;
            rectTransform.anchoredPosition = Vector3.Lerp(animasyonBaslangicPozisyon, baslangicPozisyon, t);

            yield return null;
        }

        // Tam pozisyonda ve tam görünür olduðundan emin ol
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = baslangicPozisyon;

        // Tam görünür olarak bekle
        yield return new WaitForSeconds(puanGoruntulenmeSuresi);

        // FADE OUT ANÝMASYONU
        elapsedTime = 0f;
        while (elapsedTime < puanFadeOutSuresi)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / puanFadeOutSuresi;

            // Smooth geçiþ için easing
            t = Mathf.SmoothStep(0f, 1f, t);

            // Alpha interpolasyonu (1'den 0'a)
            canvasGroup.alpha = 1f - t;

            yield return null;
        }

        // Tamamen görünmez yap ve deaktif et
        canvasGroup.alpha = 0f;
        imageMessage2.SetActive(false);
        puanMesajiGosteriliyorMu = false;

        // Coroutine referansýný temizle
        puanMesajiAnimasyonCoroutine = null;
    }

    // Satýn alma mesajý animasyonunu yöneten coroutine
    IEnumerator SatinAlmaMesajiAnimasyonu()
    {
        satinAlmaMesajiGosteriliyorMu = true;

        // Mesajý aktif et
        imageMessage3.SetActive(true);

        // Mesajýn RectTransform ve CanvasGroup bileþenlerini al
        RectTransform rectTransform = imageMessage3.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = imageMessage3.GetComponent<CanvasGroup>();

        // Eðer CanvasGroup yoksa ekle
        if (canvasGroup == null)
        {
            canvasGroup = imageMessage3.AddComponent<CanvasGroup>();
        }

        // Baþlangýç pozisyonunu kaydet
        Vector3 baslangicPozisyon = rectTransform.anchoredPosition;
        Vector3 animasyonBaslangicPozisyon = new Vector3(baslangicPozisyon.x, baslangicPozisyon.y - satinAlmaHareketMesafesi, baslangicPozisyon.z);

        // Baþlangýçta görünmez ve aþaðýda konumlandýr
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = animasyonBaslangicPozisyon;

        // FADE IN ve YUKARI HAREKET ANÝMASYONU
        float elapsedTime = 0f;
        while (elapsedTime < satinAlmaFadeInSuresi)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / satinAlmaFadeInSuresi;

            // Smooth geçiþ için easing
            t = Mathf.SmoothStep(0f, 1f, t);

            // Alpha ve pozisyon interpolasyonu
            canvasGroup.alpha = t;
            rectTransform.anchoredPosition = Vector3.Lerp(animasyonBaslangicPozisyon, baslangicPozisyon, t);

            yield return null;
        }

        // Tam pozisyonda ve tam görünür olduðundan emin ol
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = baslangicPozisyon;

        // Tam görünür olarak bekle
        yield return new WaitForSeconds(satinAlmaGoruntulenmeSuresi);

        // FADE OUT ANÝMASYONU
        elapsedTime = 0f;
        while (elapsedTime < satinAlmaFadeOutSuresi)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / satinAlmaFadeOutSuresi;

            // Smooth geçiþ için easing
            t = Mathf.SmoothStep(0f, 1f, t);

            // Alpha interpolasyonu (1'den 0'a)
            canvasGroup.alpha = 1f - t;

            yield return null;
        }

        // Tamamen görünmez yap ve deaktif et
        canvasGroup.alpha = 0f;
        imageMessage3.SetActive(false);
        satinAlmaMesajiGosteriliyorMu = false;

        // Coroutine referansýný temizle
        satinAlmaMesajiAnimasyonCoroutine = null;
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
    private void cookieTextGuncelle()
    {
        if (cookietext != null)
        {
            cookietext.text = cookiesayisi.ToString();
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
    private void cookieObjeVisibilityGuncelle()
    {
        if (cookieObjesi != null)
        {
            cookieObjesi.SetActive(cookiesayisi > 0);
        }

        // Kahve iç butonu da kahve varsa aktif
        if (cookieyeButonu != null)
        {
            cookieyeButonu.interactable = cookiesayisi > 0;
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
            cookieTextGuncelle();
            cookieObjeVisibilityGuncelle();

            // Satýn alma mesajýný göster
            SatinAlmaMesajiGoster();
        }
        else
        {
            // Yeterli puan yok uyarýsý
            StartCoroutine(YetersizPuanUyarisi());
        }
    }
    public void cookieSatinAl()
    {
        if (toplamPuan >= cookiefiyati)
        {
            toplamPuan -= cookiefiyati;
            cookiesayisi++;

            PlayerPrefs.SetInt("ToplamPuan", toplamPuan);
            PlayerPrefs.SetInt("cookiesayisi", cookiesayisi); // "cookieSayisi" yerine "cookiesayisi"
            PlayerPrefs.Save();

            // Text'leri güncelle
            PuanTextGuncelle();
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();
            cookieTextGuncelle();
            cookieObjeVisibilityGuncelle();

            // Satýn alma mesajýný göster
            SatinAlmaMesajiGoster();
        }
        else
        {
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
            cookieTextGuncelle();
            cookieObjeVisibilityGuncelle();

        }
    }
    public void cookieye()
    {
        if (cookiesayisi > 0)
        {
            // Kahve sayýsýný azalt
            cookiesayisi--;

            // Kahve içildiðinde enerjiyi %50 artýr
            if (enerjiBarScripti != null)
            {
                float enerjiArtisi = enerjiBarScripti.maxEnerji * 0.5f; // %50 hesapla
                enerjiBarScripti.mevcutEnerji += enerjiArtisi;

                // Enerji maximum deðerini aþmasýn
                if (enerjiBarScripti.mevcutEnerji > enerjiBarScripti.maxEnerji)
                {
                    enerjiBarScripti.mevcutEnerji = enerjiBarScripti.maxEnerji;
                }

                // Enerji deðerini kaydet
                PlayerPrefs.SetFloat("mevcutEnerji", enerjiBarScripti.mevcutEnerji);
                PlayerPrefs.Save();

                // Karartma kontrolünü yap (enerji yükseldiyse karartmayý temizle)
                enerjiBarScripti.KarartmaKontrolEt();
            }

            // Deðiþiklikleri kaydet
            PlayerPrefs.SetInt("KahveSayisi", kahveSayisi);
            PlayerPrefs.Save();

            // Text'leri ve görünürlüðü güncelle
            KahveTextGuncelle();
            KahveObjeVisibilityGuncelle();
            cookieTextGuncelle();
            cookieObjeVisibilityGuncelle();
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
    public GameObject hataMesajiText; // Inspector'dan baðlayacaðýnýz hata mesajý text objesi

    public void KodCalistir()
    {
        // Kullanýcýnýn yazdýðý kodu al, küçük harfe çevir (büyük-küçük harf farkýný önlemek için)
        string kod = kodAlani.text.ToLower();
        // Aktif görev indeksini al (görev yöneticisinden)
        int aktifIndex = gorevYonetici.GetAktifGorevIndex();

        bool kodDogruMu = false; // Kodun doðru olup olmadýðýný takip etmek için

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
                    kodDogruMu = true;
                }
                break;
            case 1:
                // Görev 1 için koþul:
                // Toplama iþlemi kontrolü (örneðin "5 + 3" gibi)
                if (ToplamaKontrol(kod))
                {
                    gorevYonetici.GorevBasarili();
                    PuanEkle(gorevPuani);
                    kodDogruMu = true;
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
                    kodDogruMu = true;
                }
                break;
        }

        // Eðer kod yanlýþsa hata mesajýný göster
        if (!kodDogruMu)
        {
            StartCoroutine(HataMesajiGoster());
        }
    }

    // 5 saniye boyunca hata mesajý gösteren coroutine
    private IEnumerator HataMesajiGoster()
    {
        // Eðer Inspector'da baðlanmamýþsa, isimle bul
        if (hataMesajiText == null)
        {
            hataMesajiText = GameObject.Find("HataMesajiText"); // GameObject'inizin adýný buraya yazýn
            if (hataMesajiText == null)
            {
                Debug.LogError("HataMesajiText adýnda GameObject bulunamadý!");
                yield break;
            }
        }

        // Önce Text component'ini dene
        Text textComponent = hataMesajiText.GetComponent<Text>();
        TextMeshProUGUI tmpComponent = hataMesajiText.GetComponent<TextMeshProUGUI>();

        if (textComponent == null && tmpComponent == null)
        {
            Debug.LogError("GameObject'te ne Text ne de TextMeshPro component'i bulunamadý!");
            yield break;
        }

        // Hata mesajýný göster
        if (textComponent != null)
        {
            textComponent.text = "Hatalý kod yazdýnýz!";
        }
        else if (tmpComponent != null)
        {
            tmpComponent.text = "Hatalý kod yazdýnýz!";
        }

        hataMesajiText.SetActive(true);

        // 5 saniye bekle
        yield return new WaitForSeconds(5f);

        // Mesajý gizle
        if (textComponent != null)
        {
            textComponent.text = "";
        }
        else if (tmpComponent != null)
        {
            tmpComponent.text = "";
        }

        hataMesajiText.SetActive(false);
    }

    // Puan sýfýrlama fonksiyonu (test için veya oyunu yeniden baþlatmak için)
    public void PuanSifirla()
    {
        toplamPuan = 0;
        kahveSayisi = 0;
        cookiesayisi = 0;
        PlayerPrefs.SetInt("ToplamPuan", 0);
        PlayerPrefs.SetInt("KahveSayisi", 0);
        PlayerPrefs.SetInt("cookieSayisi", 0);
        PlayerPrefs.Save();
        PuanTextGuncelle();
        KahveTextGuncelle();
        KahveObjeVisibilityGuncelle();
        cookieTextGuncelle();
        cookieObjeVisibilityGuncelle();
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
    public int GetcookieSayisi()
    {
        return cookiesayisi;
    }
}