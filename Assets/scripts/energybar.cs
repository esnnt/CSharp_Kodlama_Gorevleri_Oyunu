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

    void Start()
    {
        mevcutEnerji = PlayerPrefs.GetFloat("mevcutEnerji", maxEnerji);
        BarGuncelle();

        if (karartmaImage != null)
        {
            Color renk = karartmaImage.color;
            renk.a = 0f;          // Karartmayı görünmez yap başlangıçta
            karartmaImage.color = renk;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "bilgisayarEkrani")
        {
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            {
                mevcutEnerji -= enerjiAzalmaMiktari;
                if (mevcutEnerji < 0) mevcutEnerji = 0;

                PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);
                BarGuncelle();
            }
        }

        KarartmayiGuncelle();  // Her karede karartmayı güncelle
    }

    void BarGuncelle()
    {
        if (enerjiBarFill != null)
            enerjiBarFill.fillAmount = mevcutEnerji / maxEnerji;
    }

    void KarartmayiGuncelle()
    {
        if (karartmaImage == null) return;

        float hedefAlfa = 0f;

        if (mevcutEnerji < kararmaBaslangicEnerji)
        {
            float oran = 1f - (mevcutEnerji / kararmaBaslangicEnerji);
            hedefAlfa = Mathf.Lerp(0f, maxKararmaAlfa, oran);
        }

        Color renk = karartmaImage.color;
        renk.a = Mathf.Lerp(renk.a, hedefAlfa, Time.deltaTime * 5f);  // Yumuşak geçiş
        karartmaImage.color = renk;
    }

    public IEnumerator UyumaEnerjisiArtisi(System.Action uyumaBittiCallback)
    {
        while (mevcutEnerji < maxEnerji)
        {
            mevcutEnerji += 1f;
            if (mevcutEnerji > maxEnerji) mevcutEnerji = maxEnerji;

            PlayerPrefs.SetFloat("mevcutEnerji", mevcutEnerji);
            BarGuncelle();

            yield return new WaitForSeconds(0.5f);
        }

        uyumaBittiCallback?.Invoke();
    }
}
