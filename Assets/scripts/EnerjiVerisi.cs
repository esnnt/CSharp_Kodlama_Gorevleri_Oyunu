using UnityEngine;

// Bu attribute sayesinde Unity'de sað týklayýp "Create > OyunVerisi > Enerji" diyerek yeni bir enerji verisi dosyasý oluþturabilirsin
[CreateAssetMenu(fileName = "YeniEnerjiVerisi", menuName = "OyunVerisi/Enerji")]
public class EnerjiVerisi : ScriptableObject
{
    // Karakterin sahip olabileceði maksimum enerji deðeri 
    public float maxEnerji = 100f;

    // Karakterin þu anki mevcut enerjisi
    public float mevcutEnerji = 100f;
}
