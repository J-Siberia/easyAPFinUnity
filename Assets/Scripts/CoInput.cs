using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField coInputField; // Unityエディタ上でInputFieldを関連付ける

    private float co;

    void Start()
    {
        co = RobotController.Instance.co;
        _label.text = co.ToString();
    }

    void Update()
    {
        co = RobotController.Instance.co;
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.co = result;
        }
    }
}
