using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LgInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField lgInputField; // Unity�G�f�B�^���InputField���֘A�t����

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

    // InputField�̒l���ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.lg = result;
        }
    }
}
