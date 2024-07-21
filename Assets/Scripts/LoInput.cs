using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField loInputField; // Unityエディタ上でInputFieldを関連付ける

    private float lo;

    void Start()
    {
        lo = RobotController.Instance.lo;
        _label.text = lo.ToString();
    }

    void Update()
    {
        lo = RobotController.Instance.lo;
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.lo = result;
        }
    }
}
