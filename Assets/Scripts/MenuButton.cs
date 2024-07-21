using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void SwitchToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
