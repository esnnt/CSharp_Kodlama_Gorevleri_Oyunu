using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ErrorManager : MonoBehaviour
{
    
    // Bu script "hata ekraný" efektini yönetir.
 

    [Header("Error System Components")]
    public GameObject terminalImagePrefab; // Baþta gösterilen terminal görseli (kýsa süreli)
    public VideoPlayer terminalVideoPlayer; // Terminal animasyonu oynatacak video bileþeni
    public RawImage terminalRawImage; // Video’nun UI üzerinde görüneceði alan (RawImage UI elemaný)
    public GameObject[] errorImages = new GameObject[15]; // 15 farklý error görseli

    [Header("Timing Settings")]
    public float terminalImageDuration = 0.1f; // Terminal görseli ne kadar süreyle ekranda kalacak
    public float delayBeforeVideo = 0.5f; // Görselden sonra video baþlamadan önce ne kadar beklenmeli
    public float videoDuration = 5f; // Video kaç saniye sürecek
    public float errorSpawnInterval = 0.2f; // Error görselleri arasýndaki bekleme süresi
    public float totalErrorDuration = 20f; // Tüm error görsellerinin toplam süresi (þu an kullanýlmýyor)

    void Start()
    {
        // Oyun baþýnda tüm objeleri gizle

        if (terminalImagePrefab != null)
            terminalImagePrefab.SetActive(false);

        if (terminalVideoPlayer != null)
            terminalVideoPlayer.gameObject.SetActive(false);

        if (terminalRawImage != null)
            terminalRawImage.gameObject.SetActive(false);

        // Bütün error görsellerini en baþta sýrayla gizle
        foreach (GameObject errorImg in errorImages)
        {
            if (errorImg != null)
                errorImg.SetActive(false);
        }

        Debug.Log("ErrorManager hazýr - Tüm objeler gizlendi");
    }

    // Bu fonksiyon dýþarýdan çaðrýldýðýnda error sürecini baþlat
    public void TriggerErrorSequence()
    {
        Debug.Log("Error sequence baþlatýlýyor...");
        StartCoroutine(ErrorSequence());
    }

    // Error süreci sýralý olarak burada yürütülür
    private IEnumerator ErrorSequence()
    {
        // 1. Terminal görselini kýsa süre göster
        if (terminalImagePrefab != null)
        {
            Debug.Log("Terminal fotoðrafý gösteriliyor...");
            terminalImagePrefab.SetActive(true);
            yield return new WaitForSeconds(terminalImageDuration); // Bekle
            terminalImagePrefab.SetActive(false);
            Debug.Log("Terminal fotoðrafý gizlendi");
        }

        // 2. Video baþlamadan önce kýsa bir bekleme
        Debug.Log("Video öncesi bekleniyor...");
        yield return new WaitForSeconds(delayBeforeVideo);

        // 3. Video oynatýlýr ve eþzamanlý olarak error görselleri çýkmaya baþlar
        if (terminalVideoPlayer != null && terminalRawImage != null)
        {
            Debug.Log("Terminal videosu baþlatýlýyor...");
            terminalVideoPlayer.gameObject.SetActive(true);
            terminalRawImage.gameObject.SetActive(true);

            terminalVideoPlayer.Prepare(); // Videoyu hazýrlamaya baþla

            // Video hazýr olana kadar bekle
            while (!terminalVideoPlayer.isPrepared)
            {
                yield return null;
            }

            terminalVideoPlayer.Play(); // Videoyu oynat

            // Bu sýrada error mesajlarý gösterilmeye baþlar (ayrý bir coroutine)
            Debug.Log("Video oynarken error mesajlarý gösteriliyor...");
            StartCoroutine(ShowAllErrors());

            // Video süresi boyunca bekle
            yield return new WaitForSeconds(videoDuration);

            // Video bittikten sonra sistem kapanýr
            Debug.Log("Video bitti, sistem kapatýlýyor...");
            ShutdownSystem();
        }
    }

    // Error görsellerini sýrayla gösteren coroutine
    private IEnumerator ShowAllErrors()
    {
        for (int i = 0; i < errorImages.Length; i++)
        {
            if (errorImages[i] != null)
            {
                Debug.Log($"Error mesajý {i + 1} gösteriliyor...");
                errorImages[i].SetActive(true); // Görseli aktif et
                yield return new WaitForSeconds(errorSpawnInterval); // Bekle
            }
        }
    }

    // Tüm sistemi kapatma iþlemi (video, görseller)
    private void ShutdownSystem()
    {
        Debug.Log("Tüm objeler kapatýlýyor...");

        // Video kapatýlýr
        if (terminalVideoPlayer != null)
        {
            terminalVideoPlayer.Stop();
            terminalVideoPlayer.gameObject.SetActive(false);
        }

        if (terminalRawImage != null)
        {
            terminalRawImage.gameObject.SetActive(false);
        }

        // Aktif olan tüm error görselleri kapatýlýr
        foreach (GameObject errorImg in errorImages)
        {
            if (errorImg != null && errorImg.activeInHierarchy)
            {
                errorImg.SetActive(false);
            }
        }

        // 1 saniye sonra oyunu kapat
        StartCoroutine(QuitAfterDelay(1f));
    }

    // Belirtilen süre sonra oyunu kapatýr
    private IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Oyun kapatýlýyor...");

        Application.Quit(); // Build alýnmýþ oyunu kapatýr

#if UNITY_EDITOR
        // Unity editördeyken çalýþýyorsan oyunu durdurur
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
