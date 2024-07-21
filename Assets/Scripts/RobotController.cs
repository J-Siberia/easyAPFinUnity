using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // �V���O���g���ւ̃A�N�Z�X�|�C���g
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

    public List<Vector3> trailPoints = new List<Vector3>(); // ���S���W�̋O���_
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

        // "Barrier"�Ƃ����^�O���t���Ă���I�u�W�F�N�g���擾
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Barrier");

        // �擾�����I�u�W�F�N�g����Transform�����X�g�ɒǉ�
        foreach (GameObject obj in objects)
        {
            barrierTransforms.Add(obj.transform);
        }

        // �^�O��"Goal"�ł���I�u�W�F�N�g������
        GameObject objectGoal = GameObject.FindWithTag("Goal");

        // �I�u�W�F�N�g�����������ꍇ
        if (objectGoal != null)
        {
            // �I�u�W�F�N�g��Transform���擾
            goalTransform = objectGoal.transform;

        }
        else
        {
            // �I�u�W�F�N�g��������Ȃ������ꍇ�̏���
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
            // �ΏۃI�u�W�F�N�g�����ԋ߂��\�ʏ�̓_���擾
            Vector3 closestSurfacePoint = objCollider.ClosestPointOnBounds(robotTransform.position);
            return closestSurfacePoint;
        }
        else
        {
            // Collider���A�^�b�`����Ă��Ȃ��ꍇ�A�I�u�W�F�N�g�̈ʒu��Ԃ�
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
        // 1. ���{�b�g�̒��S���W���擾
        Vector3 robotCentralPoint = robotTransform.position;

        // �ڕW�ʒu����̈��͂�\���|�e���V��������v�Z
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

            // Ray�̌��_���I�u�W�F�N�g�̒��S�ɐݒ�
            Vector3 origin = robotTransform.position;

            // 2. �I�u�W�F�N�g2�̕\�ʏ�̓_���擾
            Vector3 hitPoint;
            // Ray���΂��A�q�b�g��������W���擾
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit))
            {
                // �q�b�g�����_�̍��W
                hitPoint = hit.point;

                // LineRenderer�̍��W��ݒ�
                lineRenderer.SetPosition(i * 2, origin);
                lineRenderer.SetPosition(i * 2 + 1, hitPoint);
            }
            else
            {
                hitPoint = robotTransform.position;

                // �q�b�g���Ȃ��ꍇ�A�K���ȋ����܂ŕ`��
                Vector3 endPoint = origin + direction * rayLength;

                // LineRenderer�̍��W��ݒ�
                lineRenderer.SetPosition(i * 2, origin);
                lineRenderer.SetPosition(i * 2 + 1, endPoint);
            }

            // 3. �I�u�W�F�N�g�Ԃ̋��������߂�
            float distance = Vector3.Distance(robotCentralPoint, hitPoint);

            // 4. �I�u�W�F�N�g���m���ŒZ�Ȓ����Ō��񂾂Ƃ��̎n�_�ƏI�_�̍��W�����߂�
            auxUrField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, 1.0f));
            auxUlField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, 1.0f));
            auxDrField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, -1.0f));
            auxDlField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, -1.0f));
            auxRightField += CalculateUo(hitPoint, new Vector3(1.0f, 0.0f, 0.0f));
            auxLeftField += CalculateUo(hitPoint, new Vector3(-1.0f, 0.0f, 0.0f));
            auxUpField += CalculateUo(hitPoint, new Vector3(0.0f, 0.0f, 1.0f));
            auxDownField += CalculateUo(hitPoint, new Vector3(0.0f, 0.0f, -1.0f));

        }

        // ����8�}�X�̃|�e���V������̒l���v�Z����
        urField = urField * ((1.0f / cg) * auxUrField + 1.0f);
        ulField = ulField * ((1.0f / cg) * auxUlField + 1.0f);
        drField = drField * ((1.0f / cg) * auxDrField + 1.0f);
        dlField = dlField * ((1.0f / cg) * auxDlField + 1.0f);
        rightField = rightField * ((1.0f / cg) * auxRightField + 1.0f);
        leftField = leftField * ((1.0f / cg) * auxLeftField + 1.0f);
        upField = upField * ((1.0f / cg) * auxUpField + 1.0f);
        downField = downField * ((1.0f / cg) * auxDownField + 1.0f);

        // �\�[�x���t�B���^�̃A�C�f�B�A�ɂ���Č��z�����߂āC���͂��Z�o����
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
