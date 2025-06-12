using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ErrorManager : MonoBehaviour
{
    
    // Bu script "hata ekran�" efektini y�netir.
 

    [Header("Error System Components")]
    public GameObject terminalImagePrefab; // Ba�ta g�sterilen terminal g�rseli (k�sa s�reli)
    public VideoPlayer terminalVideoPlayer; // Terminal animasyonu oynatacak video bile�eni
    public RawImage terminalRawImage; // Video�nun UI �zerinde g�r�nece�i alan (RawImage UI eleman�)
    public GameObject[] errorImages = new GameObject[15]; // 15 farkl� error g�rseli

    [Header("Timing Settings")]
    public float terminalImageDuration = 0.1f; // Terminal g�rseli ne kadar s�reyle ekranda kalacak
    public float delayBeforeVideo = 0.5f; // G�rselden sonra video ba�lamadan �nce ne kadar beklenmeli
    public float videoDuration = 5f; // Video ka� saniye s�recek
    public float errorSpawnInterval = 0.2f; // Error g�rselleri aras�ndaki bekleme s�resi
    public float totalErrorDuration = 20f; // T�m error g�rsellerinin toplam s�resi (�u an kullan�lm�yor)

    void Start()
    {
        // Oyun ba��nda t�m objeleri gizle

        if (terminalImagePrefab != null)
            terminalImagePrefab.SetActive(false);

        if (terminalVideoPlayer != null)
            terminalVideoPlayer.gameObject.SetActive(false);

        if (terminalRawImage != null)
            terminalRawImage.gameObject.SetActive(false);

        // B�t�n error g�rsellerini en ba�ta s�rayla gizle
        foreach (GameObject errorImg in errorImages)
        {
            if (errorImg != null)
                errorImg.SetActive(false);
        }

        Debug.Log("ErrorManager haz�r - T�m objeler gizlendi");
    }

    // Bu fonksiyon d��ar�dan �a�r�ld���nda error s�recini ba�lat
    public void TriggerErrorSequence()
    {
        Debug.Log("Error sequence ba�lat�l�yor...");
        StartCoroutine(ErrorSequence());
    }

    // Error s�reci s�ral� olarak burada y�r�t�l�r
    private IEnumerator ErrorSequence()
    {
        // 1. Terminal g�rselini k�sa s�re g�ster
        if (terminalImagePrefab != null)
        {
            Debug.Log("Terminal foto�raf� g�steriliyor...");
            terminalImagePrefab.SetActive(true);
            yield return new WaitForSeconds(terminalImageDuration); // Bekle
            terminalImagePrefab.SetActive(false);
            Debug.Log("Terminal foto�raf� gizlendi");
        }

        // 2. Video ba�lamadan �nce k�sa bir bekleme
        Debug.Log("Video �ncesi bekleniyor...");
        yield return new WaitForSeconds(delayBeforeVideo);

        // 3. Video oynat�l�r ve e�zamanl� olarak error g�rselleri ��kmaya ba�lar
        if (terminalVideoPlayer != null && terminalRawImage != null)
        {
            Debug.Log("Terminal videosu ba�lat�l�yor...");
            terminalVideoPlayer.gameObject.SetActive(true);
            terminalRawImage.gameObject.SetActive(true);

            terminalVideoPlayer.Prepare(); // Videoyu haz�rlamaya ba�la

            // Video haz�r olana kadar bekle
            while (!terminalVideoPlayer.isPrepared)
            {
                yield return null;
            }

            terminalVideoPlayer.Play(); // Videoyu oynat

            // Bu s�rada error mesajlar� g�sterilmeye ba�lar (ayr� bir coroutine)
            Debug.Log("Video oynarken error mesajlar� g�steriliyor...");
            StartCoroutine(ShowAllErrors());

            // Video s�resi boyunca bekle
            yield return new WaitForSeconds(videoDuration);

            // Video bittikten sonra sistem kapan�r
            Debug.Log("Video bitti, sistem kapat�l�yor...");
            ShutdownSystem();
        }
    }

    // Error g�rsellerini s�rayla g�steren coroutine
    private IEnumerator ShowAllErrors()
    {
        for (int i = 0; i < errorImages.Length; i++)
        {
            if (errorImages[i] != null)
            {
                Debug.Log($"Error mesaj� {i + 1} g�steriliyor...");
                errorImages[i].SetActive(true); // G�rseli aktif et
                yield return new WaitForSeconds(errorSpawnInterval); // Bekle
            }
        }
    }

    // T�m sistemi kapatma i�lemi (video, g�rseller)
    private void ShutdownSystem()
    {
        Debug.Log("T�m objeler kapat�l�yor...");

        // Video kapat�l�r
        if (terminalVideoPlayer != null)
        {
            terminalVideoPlayer.Stop();
            terminalVideoPlayer.gameObject.SetActive(false);
        }

        if (terminalRawImage != null)
        {
            terminalRawImage.gameObject.SetActive(false);
        }

        // Aktif olan t�m error g�rselleri kapat�l�r
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

    // Belirtilen s�re sonra oyunu kapat�r
    private IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Oyun kapat�l�yor...");

        Application.Quit(); // Build al�nm�� oyunu kapat�r

#if UNITY_EDITOR
        // Unity edit�rdeyken �al���yorsan oyunu durdurur
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
