using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // シングルトンへのアクセスポイント
    public static RobotController Instance { get; private set; }

    public float speed = 0.0f;
    public float cg = 1.0f;
    public float lg = 2.0f;
    public float co = 3.0f;
    public float lo = 0.2f;

    public Vector3 initPosition;

    private float urField = 0.0f;
    private float ulField = 0.0f;
    private float drField = 0.0f;
    private float dlField = 0.0f;
    private float rightField = 0.0f;
    private float leftField = 0.0f;
    private float upField = 0.0f;
    private float downField = 0.0f;
    private Vector3 force = new Vector3(0.0f, 0.0f, 0.0f);

    private Rigidbody rb;

    public Transform robotTransform;
    public Transform goalTransform;
    private List<Transform> barrierTransforms = new List<Transform>();

    public float interval = 11;
    private int numberOfRays = 0;
    public LineRenderer lineRenderer;
    public float rayLength = 10.0f;

    public List<Vector3> trailPoints = new List<Vector3>(); // 中心座標の軌道点
    private LineRenderer traceLine;

    public bool isAcc = true;

    public GameObject traceBall;
    public bool isSuccess = false;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfRays = Mathf.RoundToInt(360.0f / interval);

        rb = GetComponent<Rigidbody>();
        robotTransform = this.transform;
        initPosition = robotTransform.position;

        // "Barrier"というタグが付いているオブジェクトを取得
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Barrier");

        // 取得したオブジェクトからTransformをリストに追加
        foreach (GameObject obj in objects)
        {
            barrierTransforms.Add(obj.transform);
        }

        // タグが"Goal"であるオブジェクトを検索
        GameObject objectGoal = GameObject.FindWithTag("Goal");

        // オブジェクトが見つかった場合
        if (objectGoal != null)
        {
            // オブジェクトのTransformを取得
            goalTransform = objectGoal.transform;

        }
        else
        {
            // オブジェクトが見つからなかった場合の処理
            Debug.Log("Object with tag 'Goal' not found.");
        }

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.sortingOrder = 3;

    }

    private Vector3 GetSurfacePoint(Transform objTransform)
    {
        Collider objCollider = objTransform.GetComponent<Collider>();

        if (objCollider != null)
        {
            // 対象オブジェクトから一番近い表面上の点を取得
            Vector3 closestSurfacePoint = objCollider.ClosestPointOnBounds(robotTransform.position);
            return closestSurfacePoint;
        }
        else
        {
            // Colliderがアタッチされていない場合、オブジェクトの位置を返す
            return objTransform.position;
        }
    }

    private float CalculateUg(Vector3 offset)
    {
        float distanceSquared = Vector3.Distance(robotTransform.position + offset, goalTransform.position);
        distanceSquared *= distanceSquared;

        return cg * (1.0f - Mathf.Exp(-(distanceSquared / (lg * lg))));
    }

    private float CalculateUo(Vector3 brrvec, Vector3 offset)
    {
        float distanceSquared = Vector3.Distance(robotTransform.position + offset, brrvec);
        distanceSquared *= distanceSquared;
        return co * Mathf.Exp(-(distanceSquared / (lo * lo)));
    }

    // Update is called once per frame
    void Update()
    {
        // 1. ロボットの中心座標を取得
        Vector3 robotCentralPoint = robotTransform.position;

        // 目標位置からの引力を表すポテンシャル場を計算
        urField = CalculateUg(new Vector3(1.0f, 0.0f, 1.0f));
        ulField = CalculateUg(new Vector3(-1.0f, 0.0f, 1.0f));
        drField = CalculateUg(new Vector3(1.0f, 0.0f, -1.0f));
        dlField = CalculateUg(new Vector3(-1.0f, 0.0f, -1.0f));
        rightField = CalculateUg(new Vector3(1.0f, 0.0f, 0.0f));
        leftField = CalculateUg(new Vector3(-1.0f, 0.0f, 0.0f));
        upField = CalculateUg(new Vector3(0.0f, 0.0f, 1.0f));
        downField = CalculateUg(new Vector3(0.0f, 0.0f, -1.0f));

        float auxUrField = 0.0f;
        float auxUlField = 0.0f;
        float auxDrField = 0.0f;
        float auxDlField = 0.0f;
        float auxRightField = 0.0f;
        float auxLeftField = 0.0f;
        float auxUpField = 0.0f;
        float auxDownField = 0.0f;

        lineRenderer.positionCount = numberOfRays * 2;
        for (int i = 0; i < numberOfRays; i++)
        {
            Vector3 direction = Quaternion.Euler(0, i*interval, 0) * Vector3.forward;

            // Rayの原点をオブジェクトの中心に設定
            Vector3 origin = robotTransform.position;

            // 2. オブジェクト2の表面上の点を取得
            Vector3 hitPoint;
            // Rayを飛ばし、ヒットしたら座標を取得
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit))
            {
                // ヒットした点の座標
                hitPoint = hit.point;

                // LineRendererの座標を設定
                lineRenderer.SetPosition(i * 2, origin);
                lineRenderer.SetPosition(i * 2 + 1, hitPoint);
            }
            else
            {
                hitPoint = robotTransform.position;

                // ヒットしない場合、適当な距離まで描画
                Vector3 endPoint = origin + direction * rayLength;

                // LineRendererの座標を設定
                lineRenderer.SetPosition(i * 2, origin);
                lineRenderer.SetPosition(i * 2 + 1, endPoint);
            }

            // 3. オブジェクト間の距離を求める
            float distance = Vector3.Distance(robotCentralPoint, hitPoint);

            // 4. オブジェクト同士を最短な直線で結んだときの始点と終点の座標を求める
            auxUrField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, 1.0f));
            auxUlField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, 1.0f));
            auxDrField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, -1.0f));
            auxDlField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, -1.0f));
            auxRightField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, 0.0f));
            auxLeftField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, 0.0f));
            auxUpField += CalculateUo(hitPoint, new Vector3(0.0f, 0.0f, 1.0f));
            auxDownField += CalculateUo(hitPoint, new Vector3(0.0f, 0.0f, -1.0f));

        }

        // 周囲8マスのポテンシャル場の値を計算する
        urField = urField * ((1.0f / cg) * auxUrField + 1.0f);
        ulField = ulField * ((1.0f / cg) * auxUlField + 1.0f);
        drField = drField * ((1.0f / cg) * auxDrField + 1.0f);
        dlField = dlField * ((1.0f / cg) * auxDlField + 1.0f);
        rightField = rightField * ((1.0f / cg) * auxRightField + 1.0f);
        leftField = leftField * ((1.0f / cg) * auxLeftField + 1.0f);
        upField = upField * ((1.0f / cg) * auxUpField + 1.0f);
        downField = downField * ((1.0f / cg) * auxDownField + 1.0f);

        // ソーベルフィルタのアイディアによって勾配を求めて，合力を算出する
        force.x = -(urField + drField + rightField * 2.0f - ulField - dlField - leftField * 2.0f);
        force.z = -(urField + ulField + upField * 2.0f - drField - dlField - downField * 2.0f);
        force = force.normalized;
        //Debug.Log(force);
    }

    void FixedUpdate()
    {
        if (isAcc)
        {
            rb.AddForce(speed * force);
        }
        else
        {
            rb.velocity = speed * force;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Goal"))
        {
            //traceBall.SetActive(false);
            //gameObject.SetActive(false);
            isSuccess = true;
            lineRenderer.enabled = false;
            Debug.Log("Success!");
            Time.timeScale = 0.0f;
        }
    }
}
