using UnityEngine;
using UnityEngine.UI;

public class ErrorTriggerButton : MonoBehaviour
{
    [Header("References")]
    public ErrorManager errorManager; // ErrorManager'ý elle inspector’dan atayabilirsin

    private Button button; // Bu script’in baðlý olduðu buton bileþeni
    private bool hasTriggered = false; // Bu, butona sadece bir kez basýlmasýný saðlar

    void Start()
    {
        // Bu GameObject üzerindeki Button bileþenini bul
        button = GetComponent<Button>();

        // Eðer Button bileþeni bulunamazsa hata ver ve iþlemi durdur
        if (button == null)
        {
            //Debug.LogError("Button component bulunamadý! Bu script bir Button objesine eklenmeli.");
            return;
        }

        // Eðer errorManager sahnede atanmadýysa, sahnede otomatik olarak bul
        if (errorManager == null)
        {
            errorManager = FindObjectOfType<ErrorManager>();

            if (errorManager == null)
            {
                Debug.LogError("ErrorManager bulunamadý! Sahneye ErrorManager script'i eklediðinizden emin olun.");
                return;
            }
            else
            {
                Debug.Log("ErrorManager otomatik olarak bulundu ve atandý.");
            }
        }

        // Butonun týklama olayýna TriggerError fonksiyonunu baðla
        button.onClick.AddListener(TriggerError);

        Debug.Log("Error Trigger Button hazýr!");
    }

    // Bu fonksiyon butona týklanýnca çaðrýlýr
    void TriggerError()
    {
        // Eðer daha önce tetiklendiyse iþlemi tekrar yapma
        if (hasTriggered)
        {
            Debug.Log("Error sequence zaten çalýþýyor!");
            return;
        }

        Debug.Log("Error butonu týklandý!");

        // Butonu devre dýþý býrak, tekrar týklanmasýný engelle
        button.interactable = false;
        hasTriggered = true;

        // Eðer UI Text bileþeni varsa, yazýyý deðiþtir
        Text buttonText = GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "ERROR STARTING...";
        }

        // Eðer TextMeshPro bileþeni varsa, onun da yazýsýný deðiþtir
        TMPro.TextMeshProUGUI tmpText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = "ERROR STARTING...";
        }

        // ErrorManager'a error sekansýný baþlatmasýný söyle
        if (errorManager != null)
        {
            errorManager.TriggerErrorSequence();
        }
        else
        {
            Debug.LogError("ErrorManager referansý bulunamadý!");
            // Eðer bir þeyler ters gittiyse butonu eski haline getir
            ResetButton();
        }
    }

    // Eðer bir hata olursa butonu tekrar aktif hale getir
    private void ResetButton()
    {
        button.interactable = true;
        hasTriggered = false;

        Text buttonText = GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "TRIGGER ERROR";
        }

        TMPro.TextMeshProUGUI tmpText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = "TRIGGER ERROR";
        }
    }

    // Bu fonksiyon Unity Inspector içinden test etmek için sað týklayýp çalýþtýrýlabilir
    [ContextMenu("Test Error Trigger")]
    public void TestErrorTrigger()
    {
        TriggerError();
    }
}
