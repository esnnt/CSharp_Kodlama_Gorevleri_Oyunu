using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class energybar : MonoBehaviour
{
    // Maksimum enerji miktarı (enerji dolu haldeki değer)
    public float maxEnerji = 200f;

    // Şu anki mevcut enerji değeri
    public float mevcutEnerji;

    // Enerji barının doluluk kısmını temsil eden UI Image (fill image)
    public Image enerjiBarFill;

    // Her tuşa basıldığında enerjiden düşülecek miktar
    public float enerjiAzalmaMiktari = 0.2f;

    [Header("Kararma Ayarları")]
    // Ekranı karartan siyah image (tam ekran kaplayan, inspector'dan atanmalı)
    public Image karartmaImage;

    // Enerji bu değerin altına düşünce kararma başlayacak
    public float kararmaBaslangicEnerji = 35f;

    // Karartmanın ulaşabileceği maksimum opaklık (0 - şeffaf, 1 - tamamen siyah)
    public float maxKararmaAlfa = 0.8f;

    // Şu anki kararma opaklığı (0 ile maxKararmaAlfa arasında)
    private float currentOpacity = 0f;

    // Her tuşa basıldığında karartmanın ne kadar artacağı (opacity olarak)
    public float darknessIncreaseAmount = 2f;  // Bu değeri fazla büyük vermek opacity'yi anında 1 yapabilir.

    void Start()
    {
        // Daha önce kaydedilmiş mevcut enerji varsa onu al, yoksa maxEnerji ile başla
        float savedEnergy = PlayerPrefs.GetFloat("mevcutEnerji", maxEnerji);
        mevcutEnerji = savedEnergy;

        // Önceki karartma opaklık değeri kaydedildiyse onu al, yoksa 0
        currentOpacity = PlayerPrefs.GetFloat("currentOpacity", 0f);

        // Enerji barını güncelle
        BarGuncelle();

        // Eğer karartma image atanmışsa, kayıtlı opacity değerini uygula
        if (karartmaImage != null)
        {
            SetOpacity(currentOpacity);
        }
    }

    void Update()
    {
        // Her karede enerji barını güncelle (enerji değişse de değişmese de)
        BarGuncelle();

        // Sadece "bilgisayarEkrani" adlı sahnedeysek enerji azalt
        if (SceneManager.GetActiveScene().name == "bilgisayarEkrani")
        {
            // Eğer herhangi bir klavye tuşuna basıldıysa (fare tıklamalarını hariç tutarak)
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                // Enerjiyi azalt
                mevcutEnerji -= enerjiAzalmaMiktari;

                // Enerji 0'ın altına düşmesin
                if (mevcutEnerji < 0) mevcutEnerji = 0;

                // Güncellenen enerji değerini kaydet
                PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);

                // Eğer enerji 25'in altındaysa (düşük enerji), kararmayı artır
                if (mevcutEnerji < 25f)
                {
                    IncreaseDarkness(darknessIncreaseAmount);
                }
            }
        }
    }

    // Enerji barındaki doluluk oranını UI'da güncelleyen fonksiyon
    void BarGuncelle()
    {
        if (enerjiBarFill != null)
            enerjiBarFill.fillAmount = mevcutEnerji / maxEnerji;
    }

    // Karartma opaklığını artıran fonksiyon
    void IncreaseDarkness(float amount)
    {
        // Eğer zaten maksimum kararma seviyesine ulaşıldıysa devam etme
        if (currentOpacity >= maxKararmaAlfa) return;

        // Kararma opaklığını artır
        currentOpacity += amount;

        // Opaklık 0 ile maxKararmaAlfa arasında kalmalı, aşarsa clamp et
        currentOpacity = Mathf.Clamp(currentOpacity, 0f, maxKararmaAlfa);

        // Yeni opaklık değerini PlayerPrefs'e kaydet (kaydetmek, sahne değişince veya oyundan çıkınca devamlılık sağlar)
        PlayerPrefs.SetFloat("currentOpacity", currentOpacity);

        // Opaklık değerini karartma image'ına uygula
        SetOpacity(currentOpacity);
    }

    // Karartma image'ının alpha kanalını ayarlayan fonksiyon
    void SetOpacity(float opacity)
    {
        if (karartmaImage != null)
        {
            Color c = karartmaImage.color;
            c.a = opacity;  // Alpha değerini değiştir
            karartmaImage.color = c;
        }
    }

    // Karartmayı kontrol eden ve gerekirse temizleyen public fonksiyon
    public void KarartmaKontrolEt()
    {
        // Eğer enerji 25'in üzerindeyse karartmayı tamamen temizle
        if (mevcutEnerji >= 25f && currentOpacity > 0f)
        {
            currentOpacity = 0f;
            PlayerPrefs.SetFloat("currentOpacity", currentOpacity);
            SetOpacity(currentOpacity);
        }
    }

    // Oyuncu uyurken enerjisini yavaş yavaş artıran coroutine
    public IEnumerator UyumaEnerjisiArtisi(System.Action uyumaBittiCallback)
    {
        // Enerji maxa ulaşana kadar devam et
        while (mevcutEnerji < maxEnerji)
        {
            // Enerjiyi birim artır
            mevcutEnerji += 1f;

            // Enerji maxEnerji'yi aşmasın
            if (mevcutEnerji > maxEnerji) mevcutEnerji = maxEnerji;

            // Güncellenen enerji değerini kaydet
            PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);

            // Barı güncelle
            BarGuncelle();

            // Karartma kontrolü yap
            KarartmaKontrolEt();

            // Yarım saniye bekle, sonra tekrar devam et
            yield return new WaitForSeconds(0.5f);
        }

        // Coroutine bittiğinde callback fonksiyon varsa çağır
        uyumaBittiCallback?.Invoke();
    }
}