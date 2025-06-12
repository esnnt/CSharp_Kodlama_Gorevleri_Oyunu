using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Button için eklendi
using TMPro;
using System.Collections;

public class bilgisayarEtkilesim : MonoBehaviour
{
    [Header("F Tuþu - Bilgisayar Etkileþimi")]
    public TextMeshProUGUI fYazisi;            // Bilgisayar etkileþim mesajý için UI text
    private bool bilgisayarYakininda = false;  // Karakter bilgisayar yakýnýnda mý?

    [Header("Scene Kontrolü")]
    public string bilgisayarSceneName = "bilgisayarEkrani"; // Bilgisayar sahnesinin adý
    public string anaSceneName = "SampleScene";             // Ana sahnenin adý
    private bool bilgisayardaMi = false;                     // Karakter bilgisayar sahnesinde mi?

    [Header("E Tuþu - Uyuma Etkileþimi")]
    public TextMeshProUGUI eYazisi;               // Uyuma etkileþimi için UI text
    public TextMeshProUGUI uykuDurumuYazisi;     // Uyku durumunu gösteren UI text
    public energybar enerjiBar;                    // Enerji bar referansý
    private bool yataktaMi = false;                // Karakter yatakta mý?
    private bool uyuyor = false;                    // Karakter uyuyor mu?
    private Coroutine uyumaCoroutine;               // Uyuma coroutine referansý

    [Header("E Tuþu - Çanta Etkileþimi")]
    public TextMeshProUGUI cantaEYazisi;          // Çanta etkileþim mesajý için UI text
    public GameObject panelEnvanter;               // Envanter paneli
    public Button carpiButonu;                     // Envanter panelini kapatmak için çarpý butonu
    private bool cantaYakininda = false;           // Karakter çanta yakýnýnda mý?

    void Start()
    {
        // Baþlangýçta F ve E tuþu ile ilgili UI mesajlarýný kapat
        if (fYazisi != null) fYazisi.gameObject.SetActive(false);
        if (eYazisi != null) eYazisi.gameObject.SetActive(false);
        if (uykuDurumuYazisi != null) uykuDurumuYazisi.gameObject.SetActive(false);
        if (cantaEYazisi != null) cantaEYazisi.gameObject.SetActive(false);

        // Envanter panelini baþlangýçta kapat
        if (panelEnvanter != null) panelEnvanter.SetActive(false);

        // Eðer enerji bar atanmamýþsa sahnede bulmaya çalýþ
        if (enerjiBar == null)
            enerjiBar = FindObjectOfType<energybar>();

        // Çarpý butonuna týklama eventi ekle
        if (carpiButonu != null)
        {
            carpiButonu.onClick.AddListener(EnvanterPaneliKapat);
        }
    }

    void Update()
    {
        // Eðer bilgisayar yakýnýndaysa ve F tuþuna basýldýysa, bilgisayar sahnesine geçiþ yap
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F) && !bilgisayardaMi)
        {
            SceneManager.LoadScene(bilgisayarSceneName);
        }

        // Uyuma etkileþimi: eðer yataktaysa, E tuþuna basýldýysa ve henüz uyumuyorsa uyuma baþlat
        if (yataktaMi && Input.GetKeyDown(KeyCode.E) && !uyuyor)
        {
            if (enerjiBar != null)
            {
                uyuyor = true;  // Uyuma durumu aktif
                if (uykuDurumuYazisi != null)
                {
                    uykuDurumuYazisi.text = "Uyuyor...";
                    uykuDurumuYazisi.gameObject.SetActive(true);
                }

                // Coroutine baþlat, enerji barý artýrma iþlemi için
                uyumaCoroutine = StartCoroutine(enerjiBar.UyumaEnerjisiArtisi(() =>
                {
                    uyuyor = false; // Uyuma tamamlandý
                    if (uykuDurumuYazisi != null)
                        uykuDurumuYazisi.gameObject.SetActive(false);
                }));
            }
        }

        // Çanta etkileþimi: eðer çanta yakýnýndaysa ve E tuþuna basýldýysa envanter panelini aç/kapat
        if (cantaYakininda && Input.GetKeyDown(KeyCode.E))
        {
            if (panelEnvanter != null)
            {
                // Panel açýksa kapat, kapalýysa aç
                bool panelAktif = panelEnvanter.activeInHierarchy;
                panelEnvanter.SetActive(!panelAktif);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bilgisayar objesine girildiðinde etkileþim mesajýný göster
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = true;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(true);
        }

        // Yatak objesine girildiðinde uyuma mesajýný göster
        if (other.CompareTag("yatak"))
        {
            yataktaMi = true;
            if (eYazisi != null)
                eYazisi.gameObject.SetActive(true);

            // Henüz uyumuyorsa "Uyumak için E'ye bas" mesajý göster
            if (!uyuyor && uykuDurumuYazisi != null)
            {
                uykuDurumuYazisi.text = "Uyumak için E'ye bas";
                uykuDurumuYazisi.gameObject.SetActive(true);
            }
        }

        // Çanta objesine girildiðinde etkileþim mesajýný göster
        if (other.CompareTag("canta"))
        {
            cantaYakininda = true;
            if (cantaEYazisi != null)
            {
                cantaEYazisi.text = "Envanter için E'ye bas";
                cantaEYazisi.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Bilgisayar etkileþim bölgesinden çýkýldýðýnda mesajlarý gizle
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = false;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(false);
        }

        // Yatak bölgesinden çýkýldýðýnda uyuma durumunu ve mesajlarýný sýfýrla
        if (other.CompareTag("yatak"))
        {
            yataktaMi = false;
            uyuyor = false;

            if (eYazisi != null)
                eYazisi.gameObject.SetActive(false);
            if (uykuDurumuYazisi != null)
                uykuDurumuYazisi.gameObject.SetActive(false);

            // Uyuma coroutine'i çalýþýyorsa durdur
            if (uyumaCoroutine != null)
            {
                StopCoroutine(uyumaCoroutine);
                uyumaCoroutine = null;
            }
        }

        // Çanta etkileþim bölgesinden çýkýldýðýnda mesajý gizle
        if (other.CompareTag("canta"))
        {
            cantaYakininda = false;
            if (cantaEYazisi != null)
                cantaEYazisi.gameObject.SetActive(false);
        }
    }

    // Envanter panelini kapatmak için çarpý butonu fonksiyonu
    public void EnvanterPaneliKapat()
    {
        if (panelEnvanter != null)
        {
            panelEnvanter.SetActive(false);
        }
    }
}