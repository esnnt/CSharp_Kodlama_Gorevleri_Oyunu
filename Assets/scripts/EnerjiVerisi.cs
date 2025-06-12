using UnityEngine;

// Bu attribute sayesinde Unity'de sa� t�klay�p "Create > OyunVerisi > Enerji" diyerek yeni bir enerji verisi dosyas� olu�turabilirsin
[CreateAssetMenu(fileName = "YeniEnerjiVerisi", menuName = "OyunVerisi/Enerji")]
public class EnerjiVerisi : ScriptableObject
{
    // Karakterin sahip olabilece�i maksimum enerji de�eri (�rne�in 100)
    public float maxEnerji = 100f;

    // Karakterin �u anki mevcut enerjisi
    public float mevcutEnerji = 100f;
}
