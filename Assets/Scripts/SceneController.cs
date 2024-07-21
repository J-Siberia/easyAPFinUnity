using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SwitchTo1()
    {
        SceneManager.LoadScene("Situation1", LoadSceneMode.Single);
    }

    public void SwitchTo2()
    {
        SceneManager.LoadScene("Situation2", LoadSceneMode.Single);
    }

    public void SwitchTo3()
    {
        SceneManager.LoadScene("Situation3", LoadSceneMode.Single);
    }
}