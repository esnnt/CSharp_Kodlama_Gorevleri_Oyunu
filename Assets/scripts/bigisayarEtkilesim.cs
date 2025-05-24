using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class bilgisayarEtkilesim : MonoBehaviour
{
    [Header("F Tu�u - Bilgisayar Etkile�imi")]
    public TextMeshProUGUI fYazisi;
    private bool bilgisayarYakininda = false;

    [Header("Scene Kontrol�")]
    public string bilgisayarSceneName = "bilgisayarEkrani"; // Bilgisayar scene ad�
    public string anaSceneName = "SampleScene"; // Ana scene ad�
    private bool bilgisayardaMi = false;

    [Header("E Tu�u - Uyuma Etkile�imi")]
    public TextMeshProUGUI eYazisi;
    public TextMeshProUGUI uykuDurumuYazisi;
    public energybar enerjiBar;
    private bool yataktaMi = false;
    private bool uyuyor = false;
    private Coroutine uyumaCoroutine;

    void Start()
    {
        // UI elementlerini ba�lang��ta kapat
        if (fYazisi != null) fYazisi.gameObject.SetActive(false);
        if (eYazisi != null) eYazisi.gameObject.SetActive(false);
        if (uykuDurumuYazisi != null) uykuDurumuYazisi.gameObject.SetActive(false);

        // Enerji bar referans�n� bul
        if (enerjiBar == null)
            enerjiBar = FindObjectOfType<energybar>();
    }

    void Update()
    {
        // Bilgisayar etkile�imi
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F) && !bilgisayardaMi)
        {
            // Bilgisayar scene'ini y�kle
            SceneManager.LoadScene(bilgisayarSceneName);
        }

        // Not: ESC ile ��k�� bilgisayar scene'inde olacak

        // Uyuma Etkile�imi
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
                uykuDurumuYazisi.text = "Uyumak i�in E'ye bas";
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