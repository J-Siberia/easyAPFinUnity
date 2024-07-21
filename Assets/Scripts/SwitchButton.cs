using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    public void OnPressd()
    {
        RobotController.Instance.isAcc = (!RobotController.Instance.isAcc);
        if (!RobotController.Instance.isAcc)
        {
            label.text = "Acc";
        }
        else
        {
            label.text = "Vel";
        }
    }
}
