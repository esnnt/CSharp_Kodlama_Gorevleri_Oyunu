using UnityEngine;
using UnityEngine.SceneManagement;

public class closepanel : MonoBehaviour
{
    public void QuitToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

