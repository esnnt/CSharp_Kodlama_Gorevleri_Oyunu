using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Button i�in eklendi
using TMPro;
using System.Collections;

public class bilgisayarEtkilesim : MonoBehaviour
{
    [Header("F Tu�u - Bilgisayar Etkile�imi")]
    public TextMeshProUGUI fYazisi;            // Bilgisayar etkile�im mesaj� i�in UI text
    private bool bilgisayarYakininda = false;  // Karakter bilgisayar yak�n�nda m�?

    [Header("Scene Kontrol�")]
    public string bilgisayarSceneName = "bilgisayarEkrani"; // Bilgisayar sahnesinin ad�
    public string anaSceneName = "SampleScene";             // Ana sahnenin ad�
    private bool bilgisayardaMi = false;                     // Karakter bilgisayar sahnesinde mi?

    [Header("E Tu�u - Uyuma Etkile�imi")]
    public TextMeshProUGUI eYazisi;               // Uyuma etkile�imi i�in UI text
    public TextMeshProUGUI uykuDurumuYazisi;     // Uyku durumunu g�steren UI text
    public energybar enerjiBar;                    // Enerji bar referans�
    private bool yataktaMi = false;                // Karakter yatakta m�?
    private bool uyuyor = false;                    // Karakter uyuyor mu?
    private Coroutine uyumaCoroutine;               // Uyuma coroutine referans�

    [Header("E Tu�u - �anta Etkile�imi")]
    public TextMeshProUGUI cantaEYazisi;          // �anta etkile�im mesaj� i�in UI text
    public GameObject panelEnvanter;               // Envanter paneli
    public Button carpiButonu;                     // Envanter panelini kapatmak i�in �arp� butonu
    private bool cantaYakininda = false;           // Karakter �anta yak�n�nda m�?

    void Start()
    {
        // Ba�lang��ta F ve E tu�u ile ilgili UI mesajlar�n� kapat
        if (fYazisi != null) fYazisi.gameObject.SetActive(false);
        if (eYazisi != null) eYazisi.gameObject.SetActive(false);
        if (uykuDurumuYazisi != null) uykuDurumuYazisi.gameObject.SetActive(false);
        if (cantaEYazisi != null) cantaEYazisi.gameObject.SetActive(false);

        // Envanter panelini ba�lang��ta kapat
        if (panelEnvanter != null) panelEnvanter.SetActive(false);

        // E�er enerji bar atanmam��sa sahnede bulmaya �al��
        if (enerjiBar == null)
            enerjiBar = FindObjectOfType<energybar>();

        // �arp� butonuna t�klama eventi ekle
        if (carpiButonu != null)
        {
            carpiButonu.onClick.AddListener(EnvanterPaneliKapat);
        }
    }

    void Update()
    {
        // E�er bilgisayar yak�n�ndaysa ve F tu�una bas�ld�ysa, bilgisayar sahnesine ge�i� yap
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F) && !bilgisayardaMi)
        {
            SceneManager.LoadScene(bilgisayarSceneName);
        }

        // Uyuma etkile�imi: e�er yataktaysa, E tu�una bas�ld�ysa ve hen�z uyumuyorsa uyuma ba�lat
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

                // Coroutine ba�lat, enerji bar� art�rma i�lemi i�in
                uyumaCoroutine = StartCoroutine(enerjiBar.UyumaEnerjisiArtisi(() =>
                {
                    uyuyor = false; // Uyuma tamamland�
                    if (uykuDurumuYazisi != null)
                        uykuDurumuYazisi.gameObject.SetActive(false);
                }));
            }
        }

        // �anta etkile�imi: e�er �anta yak�n�ndaysa ve E tu�una bas�ld�ysa envanter panelini a�/kapat
        if (cantaYakininda && Input.GetKeyDown(KeyCode.E))
        {
            if (panelEnvanter != null)
            {
                // Panel a��ksa kapat, kapal�ysa a�
                bool panelAktif = panelEnvanter.activeInHierarchy;
                panelEnvanter.SetActive(!panelAktif);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bilgisayar objesine girildi�inde etkile�im mesaj�n� g�ster
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = true;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(true);
        }

        // Yatak objesine girildi�inde uyuma mesaj�n� g�ster
        if (other.CompareTag("yatak"))
        {
            yataktaMi = true;
            if (eYazisi != null)
                eYazisi.gameObject.SetActive(true);

            // Hen�z uyumuyorsa "Uyumak i�in E'ye bas" mesaj� g�ster
            if (!uyuyor && uykuDurumuYazisi != null)
            {
                uykuDurumuYazisi.text = "Uyumak i�in E'ye bas";
                uykuDurumuYazisi.gameObject.SetActive(true);
            }
        }

        // �anta objesine girildi�inde etkile�im mesaj�n� g�ster
        if (other.CompareTag("canta"))
        {
            cantaYakininda = true;
            if (cantaEYazisi != null)
            {
                cantaEYazisi.text = "Envanter i�in E'ye bas";
                cantaEYazisi.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Bilgisayar etkile�im b�lgesinden ��k�ld���nda mesajlar� gizle
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = false;
            if (fYazisi != null)
                fYazisi.gameObject.SetActive(false);
        }

        // Yatak b�lgesinden ��k�ld���nda uyuma durumunu ve mesajlar�n� s�f�rla
        if (other.CompareTag("yatak"))
        {
            yataktaMi = false;
            uyuyor = false;

            if (eYazisi != null)
                eYazisi.gameObject.SetActive(false);
            if (uykuDurumuYazisi != null)
                uykuDurumuYazisi.gameObject.SetActive(false);

            // Uyuma coroutine'i �al���yorsa durdur
            if (uyumaCoroutine != null)
            {
                StopCoroutine(uyumaCoroutine);
                uyumaCoroutine = null;
            }
        }

        // �anta etkile�im b�lgesinden ��k�ld���nda mesaj� gizle
        if (other.CompareTag("canta"))
        {
            cantaYakininda = false;
            if (cantaEYazisi != null)
                cantaEYazisi.gameObject.SetActive(false);
        }
    }

    // Envanter panelini kapatmak i�in �arp� butonu fonksiyonu
    public void EnvanterPaneliKapat()
    {
        if (panelEnvanter != null)
        {
            panelEnvanter.SetActive(false);
        }
    }
}