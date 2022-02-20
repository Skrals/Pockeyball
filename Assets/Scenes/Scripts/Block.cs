using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private ParticleSystem _missEffect;
    public void PlayMissEffect(GameObject hited)
    {
        var getPosition = hited.transform.position;
        Instantiate(_missEffect, new Vector3(getPosition.x, getPosition.y, getPosition.z - hited.transform.localScale.z), Quaternion.identity);
        _missEffect.Play();
    }

}
