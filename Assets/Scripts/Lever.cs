using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private Gate _gate;
    [SerializeField] private Transform _objectToRotate;
    [SerializeField] Vector2 _levelClampAngle = new Vector2(18, 140);
    [SerializeField] private float _pullSensitivity = 3f;
    public bool isActivated;
    private float xRotation;
    void Start()
    {
        xRotation = _levelClampAngle.x;
        _objectToRotate.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void OnMouseDrag()
    {
        if (isActivated)
            return;
        xRotation = Mathf.Clamp(xRotation - Input.GetAxis("Mouse Y") * _pullSensitivity, _levelClampAngle.x, _levelClampAngle.y);
        _objectToRotate.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if(xRotation >= _levelClampAngle.y)
        {
            isActivated = true;
            _gate.TurnOnLight();
            _gate.TryOpen();
        }
    }
}
