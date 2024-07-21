using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _label;

    private bool _isStop = false;

    // Start is called before the first frame update
    void Start()
    {
        _label.text = "Stop";
    }

    public void OnPressd()
    {
        if (_isStop)
        {
            _isStop = false;
            _label.text = "Stop";
            Time.timeScale = 1.0f;
        }
        else
        {
            _isStop = true;
            _label.text = "Start";
            Time.timeScale = 0.0f;
        }
    }
}
