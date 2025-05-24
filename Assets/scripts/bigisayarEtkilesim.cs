using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class bilgisayarEtkilesim : MonoBehaviour
{
    [Header("F Tuþu - Bilgisayar Etkileþimi")]
    public TextMeshProUGUI fYazisi;
    private bool bilgisayarYakininda = false;

    [Header("Scene Kontrolü")]
    public string bilgisayarSceneName = "bilgisayarEkrani"; // Bilgisayar scene adý
    public string anaSceneName = "SampleScene"; // Ana scene adý
    private bool bilgisayardaMi = false;

    [Header("E Tuþu - Uyuma Etkileþimi")]
    public TextMeshProUGUI eYazisi;
    public TextMeshProUGUI uykuDurumuYazisi;
    public energybar enerjiBar;
    private bool yataktaMi = false;
    private bool uyuyor = false;
    private Coroutine uyumaCoroutine;

    void Start()
    {
        // UI elementlerini baþlangýçta kapat
        if (fYazisi != null) fYazisi.gameObject.SetActive(false);
        if (eYazisi != null) eYazisi.gameObject.SetActive(false);
        if (uykuDurumuYazisi != null) uykuDurumuYazisi.gameObject.SetActive(false);

        // Enerji bar referansýný bul
        if (enerjiBar == null)
            enerjiBar = FindObjectOfType<energybar>();
    }

    void Update()
    {
        // Bilgisayar etkileþimi
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F) && !bilgisayardaMi)
        {
            // Bilgisayar scene'ini yükle
            SceneManager.LoadScene(bilgisayarSceneName);
        }

        // Not: ESC ile çýkýþ bilgisayar scene'inde olacak

        // Uyuma Etkileþimi
        if (yataktaMi && Input.GetKeyDown(KeyCode.E) && !uyuyor)
        {
            if (enerjiBar != null)
            {
                uyuyor = true;
                if (uykuDurumuYazisi != null)
                {
                    uykuDurumuYazisi.text = "Uyuyor...";
                    uykuDurumuYazisi.gameObject.SetActive(true);
                }

                uyumaCoroutine = StartCoroutine(enerjiBar.UyumaEnerjisiArtisi(() =>
                {
                    uyuyor = false;
                    if (uykuDurumuYazisi != null)
                        uykuDurumuYazisi.gameObject.SetActive(false);
                }));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = true;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(true);
        }

        if (other.CompareTag("yatak"))
        {
            yataktaMi = true;
            if (eYazisi != null)
                eYazisi.gameObject.SetActive(true);

            if (!uyuyor && uykuDurumuYazisi != null)
            {
                uykuDurumuYazisi.text = "Uyumak için E'ye bas";
                uykuDurumuYazisi.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = false;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(false);
        }

        if (other.CompareTag("yatak"))
        {
            yataktaMi = false;
            uyuyor = false;

            if (eYazisi != null)
                eYazisi.gameObject.SetActive(false);
            if (uykuDurumuYazisi != null)
                uykuDurumuYazisi.gameObject.SetActive(false);

            // Uyuma coroutine'ini durdur
            if (uyumaCoroutine != null)
            {
                StopCoroutine(uyumaCoroutine);
                uyumaCoroutine = null;
            }
        }
    }
}