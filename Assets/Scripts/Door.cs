using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : OpenableObject
{
    private Vector3 _startRotation;
    private Vector3 _targetRotation;
    [SerializeField] private float _rotateByDegrees = -90f;
    void Start()
    {
        _startRotation = transform.rotation.eulerAngles;
        _targetRotation = transform.rotation.eulerAngles + Vector3.up * _rotateByDegrees;
    }

    public override IEnumerator Open()
    {
        while (_openToCloseLerp < 1)
        {
            _openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_targetRotation), _openToCloseLerp);
            yield return null;
        }
    }

    public override IEnumerator Close()
    {
        while (_openToCloseLerp > 0)
        {
            _openToCloseLerp -= Time.deltaTime / _openCloseTime;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(_startRotation), Quaternion.Euler(_targetRotation), _openToCloseLerp);
            yield return null;
        }
    }
}
