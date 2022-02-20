using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private Stick _stick;
    [SerializeField] private Vector3 _StickSpawnOffset;

    private Rigidbody _rigidbody;
    private Stick _tmpStick;
    private Vector3 _currentPosition;
    private RaycastHit _currHit;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentPosition = transform.position;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        if (Input.GetMouseButtonDown(0))
        {
            _currentPosition = transform.position;
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.TryGetComponent(out Segment segment))
                {
                    _tmpStick = Instantiate(_stick, transform.position - _StickSpawnOffset, Quaternion.Euler(-90, -90, 90));
                    transform.SetParent(_tmpStick.BallPoint);

                    _currHit = hitInfo;

                    _rigidbody.isKinematic = true;
                    _rigidbody.velocity = Vector3.zero;
                }
                else if (hitInfo.collider.TryGetComponent(out Block block))
                {
                    _rigidbody.isKinematic = false;
                }
                else if (hitInfo.collider.TryGetComponent(out Finish finish))
                {

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            transform.SetParent(null);
            Destroy(_tmpStick.gameObject);

            if (_currHit.collider.TryGetComponent(out Segment segment))
            {
                _rigidbody.isKinematic = false;
                transform.position = _currentPosition - new Vector3(0,2,0);
                _rigidbody.AddForce(Vector3.up * (_tmpStick.GetStickPower()  * _jumpForce), ForceMode.Impulse);
            }
            else
            {
                _rigidbody.isKinematic = false;
            }
        }

    }
}
