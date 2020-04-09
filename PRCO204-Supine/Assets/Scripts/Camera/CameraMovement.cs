using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public Transform lookAt;
    public Transform cameraTransform;

    private float distance = 15f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private PlayerControls controls;

    private Vector2 cameraRot;

    void Awake()
    {
        controls = new PlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraTransform = transform;

        controls.Gameplay.CameraRotate.performed += ctx => cameraRot = ctx.ReadValue<Vector2>();
        controls.Gameplay.CameraRotate.canceled += ctx => cameraRot = Vector2.zero;
    }

    void Update() 
    {
        currentX += cameraRot.x;
        currentY += cameraRot.y;

        currentY = Mathf.Clamp(currentY, -2.5f, 50f);
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        cameraTransform.position = lookAt.position + rotation * dir;
        cameraTransform.LookAt(lookAt.position);
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
