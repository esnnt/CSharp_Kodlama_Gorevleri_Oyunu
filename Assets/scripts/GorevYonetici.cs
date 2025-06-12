using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro için

public class GorevYonetici : MonoBehaviour
{
    [Header("UI Bileþenleri")]
    public TMP_Text gorevText;           // Görev metnini gösteren TextMeshPro UI öðesi
    public List<string> gorevListesi;    // Görevlerin listesi

    [Header("Mesaj Ayarlarý")]
    public float mesajSuresi = 3f;       // Yanlýþ kod mesajýnýn gösterileceði süre (saniye)

    private int aktifGorevIndex = 0;     // Þu an aktif olan görevin indeksi
    private Color varsayilanRenk;        // Görev metninin orijinal rengi (baþlangýçta alýnýr)
    private Coroutine mesajCoroutine;    // Þu anda çalýþan mesaj gösterme coroutine'i (varsa)

    // Baþlangýçta görev metninin orijinal rengini alýr ve ilk görevi gösterir
    void Start()
    {
        varsayilanRenk = gorevText.color;
        GoreviGoster();
    }

    // Görev baþarýlý olduðunda çaðrýlýr
    public void GorevBasarili()
    {
        // Eðer yanlýþ mesaj gösteriliyorsa onu durdur
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
        }

        // Görev baþarýlý mesajý göster ve rengi yeþile ayarla
        gorevText.text = "Görev Baþarýlý!";
        gorevText.color = Color.green;

        // Bir süre bekledikten sonra sonraki göreve geç
        StartCoroutine(SonrakiGoreveGec());
    }

    // Yanlýþ kod yazýldýðýnda kullanýcýya uyarý mesajý gösterir
    public void YanlisKodMesaji(string mesaj)
    {
        // Eðer zaten mesaj gösteriliyorsa durdur (çakýþmasýn diye)
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
        }

        // Yeni mesajý gösteren coroutine'i baþlat ve referansýný sakla
        mesajCoroutine = StartCoroutine(GeciciMesajGoster(mesaj, Color.red));
    }

    // Belirtilen süre boyunca mesaj gösteren coroutine
    IEnumerator GeciciMesajGoster(string mesaj, Color renk)
    {
        // Þu anki görev metnini ve rengini kaydet (sonra geri dönecek)
        string orijinalGorev = gorevListesi[aktifGorevIndex];
        Color orijinalRenk = varsayilanRenk;

        // Yanlýþ mesajý göster ve rengi ayarla
        gorevText.text = mesaj;
        gorevText.color = renk;

        // Belirtilen süre kadar bekle (örneðin 3 saniye)
        yield return new WaitForSeconds(mesajSuresi);

        // Eski görev metnini ve rengi geri yükle
        gorevText.text = orijinalGorev;
        gorevText.color = orijinalRenk;

        // Coroutine referansýný temizle
        mesajCoroutine = null;
    }

    // Görev baþarýlý olduktan sonra biraz bekleyip sonraki göreve geçiþ yapar
    IEnumerator SonrakiGoreveGec()
    {
        yield return new WaitForSeconds(4f);  // 4 saniye bekle

        aktifGorevIndex++;  // Bir sonraki göreve geç

        if (aktifGorevIndex < gorevListesi.Count)
        {
            // Eðer görev varsa göster
            GoreviGoster();
        }
        else
        {
            // Görevler bittiðinde kullanýcýya mesaj ver
            gorevText.text = "Tüm görevler tamamlandý!";
            gorevText.color = Color.yellow;
        }
    }

    // Aktif görevi UI'da gösterir
    public void GoreviGoster()
    {
        // Eðer yanlýþ mesaj gösteriliyorsa durdur ve temizle
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
            mesajCoroutine = null;
        }

        // Þu anki görevi ve orijinal rengi ayarla
        gorevText.text = gorevListesi[aktifGorevIndex];
        gorevText.color = varsayilanRenk;
    }

    // Aktif görev indeksini döner
    public int GetAktifGorevIndex()
    {
        return aktifGorevIndex;
    }

    // Aktif görevin ismini döner
    public string GetAktifGorevAdi()
    {
        if (aktifGorevIndex < gorevListesi.Count)
        {
            return gorevListesi[aktifGorevIndex];
        }
        return "Bilinmeyen Görev";  // Eðer indeks geçerli deðilse
    }

    [Header("Debug Bilgileri")]
    [SerializeField] private int debugAktifGorevIndex; // Editörde görebilmek için

    // Update içinde debug amaçlý aktif görev indeksini günceller
    void Update()
    {
        debugAktifGorevIndex = aktifGorevIndex;
    }
}
