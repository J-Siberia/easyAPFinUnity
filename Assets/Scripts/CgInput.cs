using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CgInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField cgInputField; // Unityエディタ上でInputFieldを関連付ける

    private float cg;

    void Start()
    {
        cg = RobotController.Instance.cg;
        _label.text = cg.ToString();
    }

    void Update()
    {
        cg = RobotController.Instance.cg;
    }

    // InputFieldの値が変更されたときに呼ばれるメソッド
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.cg = result;
        }
    }
}
