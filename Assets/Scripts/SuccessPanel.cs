using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject robotObject;

    void Awake()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RobotController.Instance.isSuccess)
        {
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
