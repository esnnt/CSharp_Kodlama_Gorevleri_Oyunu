using UnityEngine;
using UnityEngine.UI;

public class kitapcikYonetici : MonoBehaviour
{
    [System.Serializable]
    public class ButonTextPair
    {
        public Button buton;
        public GameObject textIcerik; // ��erik text'ini s�r�kle (textc�kt�icerik, textdegiskenicerik vs.)
    }

    [Header("Buton ve ��erik Text �iftleri")]
    public ButonTextPair[] butonTextCiftleri;

    private int aktifIndex = -1;

    void Start()
    {
        ButonlariAyarla();
        TumTextleriGizle();
    }

    void ButonlariAyarla()
    {
        for (int i = 0; i < butonTextCiftleri.Length; i++)
        {
            if (butonTextCiftleri[i].buton != null)
            {
                int index = i;
                butonTextCiftleri[i].buton.onClick.AddListener(() => ButonaTiklandi(index));
            }
        }
    }

    void ButonaTiklandi(int index)
    {
        // Ayn� butona tekrar t�klan�rsa gizle
        if (aktifIndex == index)
        {
            TumTextleriGizle();
            aktifIndex = -1;
            return;
        }

        // T�m textleri gizle
        TumTextleriGizle();

        // Se�ilen text'i g�ster
        if (butonTextCiftleri[index].textIcerik != null)
        {
            butonTextCiftleri[index].textIcerik.SetActive(true);
            ButonRenginiAyarla(index, true);
            aktifIndex = index;
        }
    }

    void TumTextleriGizle()
    {
        for (int i = 0; i < butonTextCiftleri.Length; i++)
        {
            if (butonTextCiftleri[i].textIcerik != null)
            {
                butonTextCiftleri[i].textIcerik.SetActive(false);
            }
            ButonRenginiAyarla(i, false);
        }
    }

    void ButonRenginiAyarla(int index, bool aktif)
    {
        if (butonTextCiftleri[index].buton != null)
        {
            ColorBlock renkler = butonTextCiftleri[index].buton.colors;
            renkler.normalColor = aktif ? Color.yellow : Color.white;
            butonTextCiftleri[index].buton.colors = renkler;
        }
    }
}