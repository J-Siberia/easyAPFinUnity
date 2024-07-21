using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LgInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField lgInputField; // Unityエディタ上でInputFieldを関連付ける

    private float lg;

    void Start()
    {
        lg = RobotController.Instance.lg;
        _label.text = lg.ToString();
    }

    void Update()
    {
        lg = RobotController.Instance.lg;
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.lg = result;
        }
    }
}
