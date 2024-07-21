using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceBall : MonoBehaviour
{
    // シングルトンへのアクセスポイント
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
            // ゲームオブジェクトの中心座標を取得して経路に追加
            AddPositionToPath(transform.position);
        }
    }

    public void InitLineRenderer()
    {
        if (GetComponent<LineRenderer>() != null)
        {
            Debug.Log("Del");
            // 既にアタッチされている場合は削除
            Destroy(GetComponent<LineRenderer>());
            isLineRenderer = false;
            // 新しいフレームでLineRendererを追加
            StartCoroutine(AddLineRendererNextFrame());
        }
        else
        {
            // LineRendererコンポーネントを追加
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            isLineRenderer = true;
            // 初期設定
            lineRenderer.positionCount = 0; // 初期状態では0点
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
        // 1フレーム待つ
        yield return null;

        // 新しいフレームでLineRendererを追加
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        isLineRenderer = true;

        // 初期設定
        lineRenderer.positionCount = 0; // 初期状態では0点
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.4f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 4;
    }

    private void AddPositionToPath(Vector3 position)
    {
        // LineRendererに座標を追加
        int currentPositionCount = lineRenderer.positionCount;
        lineRenderer.positionCount = currentPositionCount + 1;
        lineRenderer.SetPosition(currentPositionCount, position);
    }
}
