using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;   //Text i�in


public class BilgisayarEtilesim : MonoBehaviour
{
    private bool bilgisayarYakininda = false;
    public TextMeshProUGUI fYazisi; //texti kullanabilmek i�in

    private void Start()
    {
        if (fYazisi != null)
            fYazisi.gameObject.SetActive(false); //yaz�y� oyun ba�lad��� zaman gizli tutuyor/pasif yap�yor(setactive kontrol�)
    }


    void Update()
    {
       
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F)) //bilgisay�n yan�ndaysa ve f ye bas�ld�ysa di�er sahneye ge�i� sa�lar
        {
            SceneManager.LoadScene("bilgisayarEkrani"); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar")) //bilgisayar�n yan�na geldiyse f ye basabilirsin mesaj� veriyor
        {
            bilgisayarYakininda = true;
            Debug.Log("Bilgisayara yakla�t�n. F'ye basabilirsin.");

            if (fYazisi != null)
                fYazisi.gameObject.SetActive(true);  //bilgisayar�n yan�na geldi�inde yaz�y� g�r�n�r yap
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = false; //bilgisayar�n yan�nda de�ilsen uzakla�t�n uyar�s� veiriyor
            Debug.Log("Bilgisayardan uzakla�t�n.");

            if (fYazisi != null)
                fYazisi.gameObject.SetActive(false); //bilgisayardan uzakla�t���nda yaz�y� gizle
        }
    }
}
