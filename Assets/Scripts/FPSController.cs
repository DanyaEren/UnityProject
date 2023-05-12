using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;

    [Header("Movement Speed Parametr's")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float runspeed = 8f;
    private float walkRunLerp;
    [SerializeField] private float walkToRunChangeSpeed = 0.5f;
    [SerializeField] private float movementSpeed;
    [Header("Camera's Parametr's")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2;
    [Header ("Jump's Parametr's")]
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float groundCheckDistance = 1f;
    void Start()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rig.angularVelocity = new Vector3(0,0,0);

        LimitRotation(mouseX, mouseY);

        Run();

        Jump();
        Debug.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance, Color.blue);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        CameraRotation(horizontal, vertical);
    }
    private void LimitRotation(float mouseX, float mouseY)
    {
        float newAngleX = cameraTransform.rotation.eulerAngles.x - mouseY * mouseSensitivity;
        if (newAngleX > 180)
        {
            newAngleX = newAngleX - 360;
        }
        newAngleX = Mathf.Clamp(newAngleX, -80, 80);
        cameraTransform.rotation = Quaternion.Euler(newAngleX, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseX * mouseSensitivity, transform.rotation.eulerAngles.z);
    }

    private void CameraRotation(float horizontal, float vertical)
    {
        Vector3 cameraForwardDir = cameraTransform.forward;
        cameraForwardDir.y = 0;

        Vector3 cameraRightDir = cameraTransform.right;
        cameraRightDir.y = 0;
        Vector3 movementDir = cameraForwardDir.normalized * vertical + cameraRightDir.normalized * horizontal;
        movementDir = Vector3.ClampMagnitude(new Vector3(movementDir.x, rig.velocity.y, movementDir.z), 1) * movementSpeed;
        rig.velocity = new Vector3(movementDir.x, rig.velocity.y, movementDir.z);
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (walkRunLerp < 1)
            {
                walkRunLerp += Time.deltaTime / walkToRunChangeSpeed;
            }
        }
        else
        {
            if (walkRunLerp > 0)
            {
                walkRunLerp -= Time.deltaTime / walkToRunChangeSpeed;
            }
        }

        movementSpeed = Mathf.Lerp(speed, runspeed, walkRunLerp);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Default")))
            {
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }
}
