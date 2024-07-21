using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField loInputField; // Unity�G�f�B�^���InputField���֘A�t����

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

    // InputField�̒l���ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.lo = result;
        }
    }
}
