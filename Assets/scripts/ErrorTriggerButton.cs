using UnityEngine;
using UnityEngine.UI;

public class ErrorTriggerButton : MonoBehaviour
{
    [Header("References")]
    public ErrorManager errorManager; // ErrorManager'� elle inspector�dan atayabilirsin

    private Button button; // Bu script�in ba�l� oldu�u buton bile�eni
    private bool hasTriggered = false; // Bu, butona sadece bir kez bas�lmas�n� sa�lar

    void Start()
    {
        // Bu GameObject �zerindeki Button bile�enini bul
        button = GetComponent<Button>();

        // E�er Button bile�eni bulunamazsa hata ver ve i�lemi durdur
        if (button == null)
        {
            //Debug.LogError("Button component bulunamad�! Bu script bir Button objesine eklenmeli.");
            return;
        }

        // E�er errorManager sahnede atanmad�ysa, sahnede otomatik olarak bul
        if (errorManager == null)
        {
            errorManager = FindObjectOfType<ErrorManager>();

            if (errorManager == null)
            {
                Debug.LogError("ErrorManager bulunamad�! Sahneye ErrorManager script'i ekledi�inizden emin olun.");
                return;
            }
            else
            {
                Debug.Log("ErrorManager otomatik olarak bulundu ve atand�.");
            }
        }

        // Butonun t�klama olay�na TriggerError fonksiyonunu ba�la
        button.onClick.AddListener(TriggerError);

        Debug.Log("Error Trigger Button haz�r!");
    }

    // Bu fonksiyon butona t�klan�nca �a�r�l�r
    void TriggerError()
    {
        // E�er daha �nce tetiklendiyse i�lemi tekrar yapma
        if (hasTriggered)
        {
            Debug.Log("Error sequence zaten �al���yor!");
            return;
        }

        Debug.Log("Error butonu t�kland�!");

        // Butonu devre d��� b�rak, tekrar t�klanmas�n� engelle
        button.interactable = false;
        hasTriggered = true;

        // E�er UI Text bile�eni varsa, yaz�y� de�i�tir
        Text buttonText = GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = "ERROR STARTING...";
        }

        // E�er TextMeshPro bile�eni varsa, onun da yaz�s�n� de�i�tir
        TMPro.TextMeshProUGUI tmpText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = "ERROR STARTING...";
        }

        // ErrorManager'a error sekans�n� ba�latmas�n� s�yle
        if (errorManager != null)
        {
            errorManager.TriggerErrorSequence();
        }
        else
        {
            Debug.LogError("ErrorManager referans� bulunamad�!");
            // E�er bir �eyler ters gittiyse butonu eski haline getir
            ResetButton();
        }
    }

    // E�er bir hata olursa butonu tekrar aktif hale getir
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

    // Bu fonksiyon Unity Inspector i�inden test etmek i�in sa� t�klay�p �al��t�r�labilir
    [ContextMenu("Test Error Trigger")]
    public void TestErrorTrigger()
    {
        TriggerError();
    }
}
