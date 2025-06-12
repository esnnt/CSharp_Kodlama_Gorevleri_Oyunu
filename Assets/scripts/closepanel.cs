using UnityEngine;
using UnityEngine.SceneManagement;

public class closepanel : MonoBehaviour
{
    // Bu fonksiyon çaðrýldýðýnda "SampleScene" adlý sahne yüklenir.
    public void QuitToSampleScene()
    {
        // SceneManager sýnýfý ile sahne geçiþi yapýlýyor.
        SceneManager.LoadScene("SampleScene");
    }
}
