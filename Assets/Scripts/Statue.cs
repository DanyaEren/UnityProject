using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : OpenableObject
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    [SerializeField] private float _moveDown = 5f;

    [SerializeField] private GameObject _exit;

    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position - transform.up * _moveDown;
    }

    public IEnumerator GoDown()
    {
        while(_openToCloseLerp < 1)
        {
            _openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _openToCloseLerp);
            yield return null;
        }

        _exit.SetActive(true);
    }
}
