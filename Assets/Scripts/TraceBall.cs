using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceBall : MonoBehaviour
{
    // �V���O���g���ւ̃A�N�Z�X�|�C���g
    public static TraceBall Instance { get; private set; }

    public Vector3 initPosition;

    public LineRenderer lineRenderer;
    private bool isLineRenderer = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitLineRenderer();
        initPosition = this.transform.position;
    }

    void Update()
    {
        if (isLineRenderer)
        {
            // �Q�[���I�u�W�F�N�g�̒��S���W���擾���Čo�H�ɒǉ�
            AddPositionToPath(transform.position);
        }
    }

    public void InitLineRenderer()
    {
        if (GetComponent<LineRenderer>() != null)
        {
            Debug.Log("Del");
            // ���ɃA�^�b�`����Ă���ꍇ�͍폜
            Destroy(GetComponent<LineRenderer>());
            isLineRenderer = false;
            // �V�����t���[����LineRenderer��ǉ�
            StartCoroutine(AddLineRendererNextFrame());
        }
        else
        {
            // LineRenderer�R���|�[�l���g��ǉ�
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            isLineRenderer = true;
            // �����ݒ�
            lineRenderer.positionCount = 0; // ������Ԃł�0�_
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.4f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.sortingOrder = 4;
        }
    }

    IEnumerator AddLineRendererNextFrame()
    {
        // 1�t���[���҂�
        yield return null;

        // �V�����t���[����LineRenderer��ǉ�
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        isLineRenderer = true;

        // �����ݒ�
        lineRenderer.positionCount = 0; // ������Ԃł�0�_
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.4f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 4;
    }

    private void AddPositionToPath(Vector3 position)
    {
        // LineRenderer�ɍ��W��ǉ�
        int currentPositionCount = lineRenderer.positionCount;
        lineRenderer.positionCount = currentPositionCount + 1;
        lineRenderer.SetPosition(currentPositionCount, position);
    }
}
