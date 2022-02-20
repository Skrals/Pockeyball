using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private Stick _stick;
    [SerializeField] private Vector3 _StickSpawnOffset;
    [SerializeField] private bool _controllerCD;

    private Rigidbody _rigidbody;
    private Stick _tmpStick;
    private Vector3 _currentPosition;
    private RaycastHit _currHit;

    private void Start()
    {
        _controllerCD = false;
        _rigidbody = GetComponent<Rigidbody>();
        _currentPosition = transform.position;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);
        if (Input.GetMouseButtonDown(0) &&  !_controllerCD)
        {
            StartCoroutine(CD());
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
                    _tmpStick = Instantiate(_stick, transform.position - _StickSpawnOffset, Quaternion.Euler(-90, -90, 90));

                    _currHit = hitInfo;

                    block.PlayMissEffect(block.gameObject);
                    StartCoroutine(DeleteStick());
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

    private IEnumerator DeleteStick ()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(_tmpStick.gameObject);
        StopCoroutine(DeleteStick());
    }

    private IEnumerator CD ()
    {
        _controllerCD = true;
        yield return new WaitForSeconds(1);
        _controllerCD = false;
        StopCoroutine(CD());
    }
}
