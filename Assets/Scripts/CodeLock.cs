using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeLock : MonoBehaviour
{
    [SerializeField] private OpenableObject _openableObject;
    [SerializeField] private string _code = "1234";
    private string _typedCode = "";
    [SerializeField] TMP_Text _codeLockText;
    [SerializeField] private MeshRenderer _successIndicator;
    [SerializeField] private Camera _codeLockCamera;
    [SerializeField] private CameraSwitcher _cameraSwitcher;
    void Start()
    {
        _openableObject.Lock(this);
        _successIndicator.material.SetColor("_EmissionColor", Color.red);
    }

    public void AddNumber(string newNumber)
    {
        if (_typedCode.Length < 4)
        {
            SetTypedCode(_typedCode + newNumber);
        }
    }

    public void ClearNumbers()
    {
        SetTypedCode("");
    }

    private void SetTypedCode(string newTypedCode)
    {
        _typedCode = newTypedCode;
        _codeLockText.text = _typedCode;
    }

    public void CheckCode()
    {
        if (_code.Equals(_typedCode))
        {
            _openableObject.Unlock();
            _successIndicator.material.SetColor("_EmissionColor", Color.green);
            StartCoroutine(_cameraSwitcher.BackToMainCameraWithDelay());
        }
        else
        {
            ClearNumbers();
        }
    }

    public void SetCodeLockCameraAsMain()
    {
        _codeLockCamera.depth = 10f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _cameraSwitcher.SetOtherCamera(_codeLockCamera);
    }
}
