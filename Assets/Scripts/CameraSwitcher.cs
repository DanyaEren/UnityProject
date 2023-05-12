using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private bool _isUsingMainCamera = true;
    private Camera _otherCamera;
    [SerializeField] private FPSController _fpsController;
    public Camera mainCamera;

    void Update()
    {
        if(!_isUsingMainCamera)
        {
            if (Input.GetMouseButtonDown(1))
            {
                BackToMainCamera();
            }
        }
    }

    public void SetOtherCamera(Camera newCamera)
    {
        _isUsingMainCamera = false;
        _otherCamera = newCamera;
        _fpsController.enabled = false;
        
    }

    public void BackToMainCamera()
    {
        _otherCamera.depth = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isUsingMainCamera = true;
        _fpsController.enabled = true;
    }

    public IEnumerator BackToMainCameraWithDelay()
    {
        yield return new WaitForSeconds(1f);
        BackToMainCamera();
    }
}
