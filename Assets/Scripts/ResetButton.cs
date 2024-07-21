using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public GameObject robotObject;
    public GameObject traceBall;

    public void OnPressd()
    {
        if (robotObject != null)
        {
            if (RobotController.Instance.isSuccess)
            {
                //robotObject.SetActive(true);
                //traceBall.SetActive(true);
                RobotController.Instance.isSuccess = false;
                RobotController.Instance.lineRenderer.enabled = true;
                Time.timeScale = 1.0f;
            }
            RobotController.Instance.trailPoints = new List<Vector3>();
            robotObject.transform.position = RobotController.Instance.initPosition;
            traceBall.transform.position = TraceBall.Instance.initPosition;

            // LineRendererコンポーネントの初期設定(traceBall)
            TraceBall.Instance.InitLineRenderer();
        }
    }
}
