using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField coInputField; // Unity�G�f�B�^���InputField���֘A�t����

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

    // InputField�̒l���ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.co = result;
        }
    }
}
