using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBarrier : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMagnitude = 7.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned!");
            return;
        }
    }

    void FixedUpdate()
    {
        // ƒ‰ƒ“ƒ_ƒ€‚È•ûŒü‚Ì—Í‚ð‰Á‚¦‚é
        Vector3 randomForceDirection = Random.onUnitSphere;
        randomForceDirection.y = 0.0f;
        randomForceDirection.Normalize();

        rb.AddForce(randomForceDirection * forceMagnitude, ForceMode.Impulse);
    }
}
