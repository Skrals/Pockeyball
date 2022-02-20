using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _ballPoint;
    [SerializeField] private float _power;

    public Transform BallPoint => _ballPoint;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _power += Time.deltaTime;
        }
        _power = Mathf.Clamp(_power, 0, 1);
        _animator.SetFloat("Blend", _power);
    }

    public float GetStickPower()
    {
        return _power;
    }
}
