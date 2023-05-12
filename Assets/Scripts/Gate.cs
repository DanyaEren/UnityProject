using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : OpenableObject
{
    [SerializeField] private Material _glowMaterial;
    [SerializeField] private List<MeshRenderer> _lights;
    [SerializeField] private List<Lever> _levers;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    [SerializeField] private float _moveDown = 5f;
    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position - transform.up * _moveDown;   
    }

    public void TryOpen()
    {
        foreach (var lever in _levers)
        {
            if (!lever.isActivated)
                return;
        }
        StartCoroutine(GoDown());
    }

    public void TurnOnLight()
    {
        foreach (var light in _lights)
        {
            if(light.material.GetColor("_EmissionColor") != _glowMaterial.GetColor("_EmissionColor"))
            {
                light.material = _glowMaterial;
                return;
            }
        }
    }

    public IEnumerator GoDown()
    {
        while (_openToCloseLerp < 1)
        {
            _openToCloseLerp += Time.deltaTime / _openCloseTime;
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, _openToCloseLerp);
            yield return null;
        }
    }
}
