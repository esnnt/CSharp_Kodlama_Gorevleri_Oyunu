using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;   //Text için


public class BilgisayarEtilesim : MonoBehaviour
{
    private bool bilgisayarYakininda = false;
    public TextMeshProUGUI fYazisi; //texti kullanabilmek için

    private void Start()
    {
        if (fYazisi != null)
            fYazisi.gameObject.SetActive(false); //yazýyý oyun baþladýðý zaman gizli tutuyor/pasif yapýyor(setactive kontrolü)
    }


    void Update()
    {
       
        if (bilgisayarYakininda && Input.GetKeyDown(KeyCode.F)) //bilgisayýn yanýndaysa ve f ye basýldýysa diðer sahneye geçiþ saðlar
        {
            SceneManager.LoadScene("bilgisayarEkrani"); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar")) //bilgisayarýn yanýna geldiyse f ye basabilirsin mesajý veriyor
        {
            bilgisayarYakininda = true;
            Debug.Log("Bilgisayara yaklaþtýn. F'ye basabilirsin.");

            if (fYazisi != null)
                fYazisi.gameObject.SetActive(true);  //bilgisayarýn yanýna geldiðinde yazýyý görünür yap
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("bilgisayar"))
        {
            bilgisayarYakininda = false; //bilgisayarýn yanýnda deðilsen uzaklaþtýn uyarýsý veiriyor
            Debug.Log("Bilgisayardan uzaklaþtýn.");

            if (fYazisi != null)
                fYazisi.gameObject.SetActive(false); //bilgisayardan uzaklaþtýðýnda yazýyý gizle
        }
    }
}
