using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshPro i�in

public class GorevYonetici : MonoBehaviour
{
    [Header("UI Bile�enleri")]
    public TMP_Text gorevText;           // G�rev metnini g�steren TextMeshPro UI ��esi
    public List<string> gorevListesi;    // G�revlerin listesi

    [Header("Mesaj Ayarlar�")]
    public float mesajSuresi = 3f;       // Yanl�� kod mesaj�n�n g�sterilece�i s�re (saniye)

    private int aktifGorevIndex = 0;     // �u an aktif olan g�revin indeksi
    private Color varsayilanRenk;        // G�rev metninin orijinal rengi (ba�lang��ta al�n�r)
    private Coroutine mesajCoroutine;    // �u anda �al��an mesaj g�sterme coroutine'i (varsa)

    // Ba�lang��ta g�rev metninin orijinal rengini al�r ve ilk g�revi g�sterir
    void Start()
    {
        varsayilanRenk = gorevText.color;
        GoreviGoster();
    }

    // G�rev ba�ar�l� oldu�unda �a�r�l�r
    public void GorevBasarili()
    {
        // E�er yanl�� mesaj g�steriliyorsa onu durdur
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
        }

        // G�rev ba�ar�l� mesaj� g�ster ve rengi ye�ile ayarla
        gorevText.text = "G�rev Ba�ar�l�!";
        gorevText.color = Color.green;

        // Bir s�re bekledikten sonra sonraki g�reve ge�
        StartCoroutine(SonrakiGoreveGec());
    }

    // Yanl�� kod yaz�ld���nda kullan�c�ya uyar� mesaj� g�sterir
    public void YanlisKodMesaji(string mesaj)
    {
        // E�er zaten mesaj g�steriliyorsa durdur (�ak��mas�n diye)
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
        }

        // Yeni mesaj� g�steren coroutine'i ba�lat ve referans�n� sakla
        mesajCoroutine = StartCoroutine(GeciciMesajGoster(mesaj, Color.red));
    }

    // Belirtilen s�re boyunca mesaj g�steren coroutine
    IEnumerator GeciciMesajGoster(string mesaj, Color renk)
    {
        // �u anki g�rev metnini ve rengini kaydet (sonra geri d�necek)
        string orijinalGorev = gorevListesi[aktifGorevIndex];
        Color orijinalRenk = varsayilanRenk;

        // Yanl�� mesaj� g�ster ve rengi ayarla
        gorevText.text = mesaj;
        gorevText.color = renk;

        // Belirtilen s�re kadar bekle (�rne�in 3 saniye)
        yield return new WaitForSeconds(mesajSuresi);

        // Eski g�rev metnini ve rengi geri y�kle
        gorevText.text = orijinalGorev;
        gorevText.color = orijinalRenk;

        // Coroutine referans�n� temizle
        mesajCoroutine = null;
    }

    // G�rev ba�ar�l� olduktan sonra biraz bekleyip sonraki g�reve ge�i� yapar
    IEnumerator SonrakiGoreveGec()
    {
        yield return new WaitForSeconds(4f);  // 4 saniye bekle

        aktifGorevIndex++;  // Bir sonraki g�reve ge�

        if (aktifGorevIndex < gorevListesi.Count)
        {
            // E�er g�rev varsa g�ster
            GoreviGoster();
        }
        else
        {
            // G�revler bitti�inde kullan�c�ya mesaj ver
            gorevText.text = "T�m g�revler tamamland�!";
            gorevText.color = Color.yellow;
        }
    }

    // Aktif g�revi UI'da g�sterir
    public void GoreviGoster()
    {
        // E�er yanl�� mesaj g�steriliyorsa durdur ve temizle
        if (mesajCoroutine != null)
        {
            StopCoroutine(mesajCoroutine);
            mesajCoroutine = null;
        }

        // �u anki g�revi ve orijinal rengi ayarla
        gorevText.text = gorevListesi[aktifGorevIndex];
        gorevText.color = varsayilanRenk;
    }

    // Aktif g�rev indeksini d�ner
    public int GetAktifGorevIndex()
    {
        return aktifGorevIndex;
    }

    // Aktif g�revin ismini d�ner
    public string GetAktifGorevAdi()
    {
        if (aktifGorevIndex < gorevListesi.Count)
        {
            return gorevListesi[aktifGorevIndex];
        }
        return "Bilinmeyen G�rev";  // E�er indeks ge�erli de�ilse
    }

    [Header("Debug Bilgileri")]
    [SerializeField] private int debugAktifGorevIndex; // Edit�rde g�rebilmek i�in

    // Update i�inde debug ama�l� aktif g�rev indeksini g�nceller
    void Update()
    {
        debugAktifGorevIndex = aktifGorevIndex;
    }
}
