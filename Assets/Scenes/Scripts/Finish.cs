using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Ball ball))
        {
            var rigidbody = ball.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            ball.enabled = false;
        }
    }
}
