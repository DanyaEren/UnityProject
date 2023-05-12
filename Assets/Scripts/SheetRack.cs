using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetRack : OpenableObject
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    [SerializeField] private float _moveByForward = 0.4f;
    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position - transform.forward * _moveByForward;
    }

    public override IEnumerator Open()
    {
        while (_openToCloseLerp < 1)
        {
            _openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _openToCloseLerp);
            yield return null;
        }
    }

    public override IEnumerator Close()
    {
        while (_openToCloseLerp > 0)
        {
            _openToCloseLerp -= Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _openToCloseLerp);
            yield return null;
        }
    }
   
}
