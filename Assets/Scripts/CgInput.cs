using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CgInput : MonoBehaviour
{
    public TextMeshProUGUI _label;
    public InputField cgInputField; // Unity�G�f�B�^���InputField���֘A�t����

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

    // InputField�̒l���ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnInputFieldValueChanged(string value)
    {
        if (float.TryParse(value, out float result))
        {
            RobotController.Instance.cg = result;
        }
    }
}
