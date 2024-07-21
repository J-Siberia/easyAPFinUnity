using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField speedInputField; // Unityエディタ上でInputFieldを関連付ける

    private float speed;

    void Start()
    {
        speed = RobotController.Instance.speed;
    }

    void Update()
    {
        speed = RobotController.Instance.speed;
        _label.text = RobotController.Instance.speed.ToString();
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.speed = result;
        }
    }
}
