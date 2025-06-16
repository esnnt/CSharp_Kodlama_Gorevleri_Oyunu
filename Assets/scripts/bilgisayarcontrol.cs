using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bilgisayarcontrol : MonoBehaviour
{
    [Header("Menü Kontrolü")]
    public GameObject menuPanel;        // Ana menü paneli (play/exit butonlarý olan)
    public Image karartmaImage;         // Karartma image'i
    public Button playButton;           // Play butonu
    public Button exitButton;           // Exit butonu
    public float fadeDuration = 2f;     // Geçiþ süresi

    [Header("Bilgisayar Panel Kontrolü")]
    public GameObject bilgisayarPanel;  // Canvas2 altýnda bulunan bilgisayar paneli (GameObject)
    [Header("Yeni Panel Kontrolü")]
    public GameObject yeniPanel;  // Yeni eklenen panel (butona basýnca açýlacak)
    [Header("kitapcik Panel Kontrolü")]
    public GameObject kitapcikPanel;  // Yeni eklenen panel (butona basýnca açýlacak)

    private bool isTransitioning = false; // Geçiþ durumu kontrolü
    private bool gameStarted = false;     // Oyun baþladý mý?

    // Baþlangýçta tüm oyun panellerini kapatýr, sadece menüyü gösterir
    void Start()
    {
        // Oyun panellerini kapat
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
        if (yeniPanel != null)
        {
            yeniPanel.SetActive(false);
        }
        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(false);
        }

        // Menü kontrolü
        if (menuPanel != null)
        {
            menuPanel.SetActive(true); // Menü paneli açýk
        }

        // Karartma image baþlangýçta tam opak ve en arkada
        if (karartmaImage != null)
        {
            SetImageAlpha(karartmaImage, 1f);
            karartmaImage.transform.SetAsFirstSibling(); // En arkaya gönder
        }

        // Buton event'lerini baðla
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }

        gameStarted = false;
    }

    #region Menü Kontrolü
    public void PlayGame()
    {
        if (isTransitioning || gameStarted) return;

        Debug.Log("Play butonuna basýldý - Oyun açýlýyor...");

        // Butonlarý deaktif et
        if (playButton != null) playButton.interactable = false;
        if (exitButton != null) exitButton.interactable = false;

        // Karartmayý yavaþça kaldýr ve oyunu baþlat
        StartCoroutine(FadeOutAndStartGame());
    }

    public void ExitGame()
    {
        if (isTransitioning) return;

        Debug.Log("Exit butonuna basýldý - Oyun kapatýlýyor...");

        // Butonlarý deaktif et
        if (playButton != null) playButton.interactable = false;
        if (exitButton != null) exitButton.interactable = false;

        // Karartmayý yavaþça artýr ve oyunu kapat
        StartCoroutine(FadeInAndExit());
    }

    private IEnumerator FadeOutAndStartGame()
    {
        isTransitioning = true;

        // Önce menü panelini kapat
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // Oyun baþladý olarak iþaretle
        gameStarted = true;

        Debug.Log("Menü kapandý - Oyun açýlýyor...");

        // Ýsteðe baðlý: Oyun baþladýðýnda bir panel açmak istersen
        // Ac(); // Örneðin bilgisayar panelini aç

        // Karartmayý yavaþça kaldýr (oyun ekraný siyahtan aydýnlýða)
        float startAlpha = karartmaImage != null ? karartmaImage.color.a : 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            if (karartmaImage != null)
            {
                SetImageAlpha(karartmaImage, currentAlpha);
            }
            yield return null;
        }

        // Karartmayý tamamen kaldýr
        if (karartmaImage != null)
        {
            SetImageAlpha(karartmaImage, 0f);
        }

        isTransitioning = false;

        Debug.Log("Karartma kaldýrýldý - Oyun tamamen aydýnlandý!");
    }

    private IEnumerator FadeInAndExit()
    {
        isTransitioning = true;

        float startAlpha = karartmaImage != null ? karartmaImage.color.a : 0f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            if (karartmaImage != null)
            {
                SetImageAlpha(karartmaImage, currentAlpha);
            }
            yield return null;
        }

        // Karartmayý tam opak yap
        if (karartmaImage != null)
        {
            SetImageAlpha(karartmaImage, 1f);
        }

        Debug.Log("Karartma tamamlandý - Oyun kapatýlýyor...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    // Oyunu menüye geri döndürmek için (isteðe baðlý)
    public void BackToMenu()
    {
        if (isTransitioning) return;

        // Tüm oyun panellerini kapat
        Kapat();
        YeniPanelKapat();
        KitapcikPanelKapat();

        // Menüyü göster
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }

        // Butonlarý aktif et
        if (playButton != null) playButton.interactable = true;
        if (exitButton != null) exitButton.interactable = true;

        // Karartmayý tekrar tam opak yap
        if (karartmaImage != null)
        {
            SetImageAlpha(karartmaImage, 1f);
        }

        gameStarted = false;
    }
    #endregion

    #region Mevcut Panel Kontrolü (Deðiþtirilmedi)
    // Bilgisayar panelini kapatan fonksiyon (örneðin buton ile çaðrýlýr)
    public void Kapat()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
    }
    // Bilgisayar panelini açan fonksiyon (örneðin buton ile çaðrýlýr)
    public void Ac()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(true);
        }
    }
    public void EKapat()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(false);
        }
    }
    public void EAc()
    {
        if (bilgisayarPanel != null)
        {
            bilgisayarPanel.SetActive(true);
        }
    }
    // Yeni panel'i açan fonksiyon
    public void YeniPanelAc()
    {
        if (yeniPanel != null)
        {
            yeniPanel.SetActive(true);
        }
    }
    // Yeni panel'i kapatan fonksiyon
    public void YeniPanelKapat()
    {
        if (yeniPanel != null)
        {
            yeniPanel.SetActive(false);
        }
    }
    public void KitapcikPanelAc()
    {
        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(true);
        }
    }
    public void KitapcikPanelKapat()
    {
        if (kitapcikPanel != null)
        {
            kitapcikPanel.SetActive(false);
        }
    }
    // Sahnedeki tüm TMP_InputField'larý bulur ve içindeki yazýyý temizler
    public void ClearAllInputs()
    {
        // Sahnedeki tüm TextMeshPro input alanlarýný bul
        TMP_InputField[] inputs = FindObjectsOfType<TMP_InputField>();
        // Her bir input alanýný sýfýrla
        foreach (TMP_InputField input in inputs)
        {
            if (input != null)
            {
                input.text = "";    // Text'i boþ yap
            }
        }
    }
    #endregion
}