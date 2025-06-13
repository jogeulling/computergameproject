using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class EndScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }
}
