using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class energybar : MonoBehaviour
{
    public float maxEnerji = 200f;
    public float mevcutEnerji;
    public Image enerjiBarFill;
    public float enerjiAzalmaMiktari = 0.2f;

    [Header("Kararma Ayarları")]
    public Image karartmaImage;               // Inspector'dan atamalısın
    public float kararmaBaslangicEnerji = 35f;
    public float maxKararmaAlfa = 0.8f;

    // Karartma için yeni değişkenler
    private float currentOpacity = 0f;
    public float darknessIncreaseAmount = 3f; // Her tuşta artacak kararma miktarı


    void Start()
    {

        float savedEnergy = PlayerPrefs.GetFloat("mevcutEnerji", maxEnerji);
        mevcutEnerji = savedEnergy;
        currentOpacity = PlayerPrefs.GetFloat("currentOpacity", 0f);
        BarGuncelle();
        if (karartmaImage != null)
        {
            SetOpacity(currentOpacity);
        }
    }
    void Update()
    {
        // Her sahnede bar güncellemesini kontrol et
        BarGuncelle();

        // Sadece bilgisayar ekranında enerji azalt
        if (SceneManager.GetActiveScene().name == "bilgisayarEkrani")
        {
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                mevcutEnerji -= enerjiAzalmaMiktari;
                if (mevcutEnerji < 0) mevcutEnerji = 0;
                PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);

                // Enerji 75'in altındaysa karartmayı artır
                if (mevcutEnerji < 25f)
                {
                    IncreaseDarkness(darknessIncreaseAmount);
                }
            }
        }
    }

    void BarGuncelle()
    {
        if (enerjiBarFill != null)
            enerjiBarFill.fillAmount = mevcutEnerji / maxEnerji;
    }

    // Karartma fonksiyonları (EnergyDarkness'tan alındı)
    void IncreaseDarkness(float amount)
    {
        if (currentOpacity >= maxKararmaAlfa) return;
        currentOpacity += amount;
        currentOpacity = Mathf.Clamp(currentOpacity, 0f, maxKararmaAlfa);

        // Opacity'yi kaydet
        PlayerPrefs.SetFloat("currentOpacity", currentOpacity);

        SetOpacity(currentOpacity);
    }

    void SetOpacity(float opacity)
    {
        if (karartmaImage != null)
        {
            Color c = karartmaImage.color;
            c.a = opacity;
            karartmaImage.color = c;
        }
    }

    public IEnumerator UyumaEnerjisiArtisi(System.Action uyumaBittiCallback)
    {
        while (mevcutEnerji < maxEnerji)
        {
            mevcutEnerji += 1f;
            if (mevcutEnerji > maxEnerji) mevcutEnerji = maxEnerji;
            PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);
            BarGuncelle();

            // Enerji 75'e ulaştığında karartmayı temizle
            if (mevcutEnerji >= 25f && currentOpacity > 0f)
            {
                currentOpacity = 0f;
                PlayerPrefs.SetFloat("currentOpacity", currentOpacity);
                SetOpacity(currentOpacity);
            }


            yield return new WaitForSeconds(0.5f);
        }
        uyumaBittiCallback?.Invoke();
    }
}