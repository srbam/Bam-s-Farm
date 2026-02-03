using NUnit.Framework.Constraints;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraControl : MonoBehaviour
{
    private Camera cam;

    public InputAction moveAction;
    public InputAction zoomAction;

    public float moveSpeed = 10f;
    public float zoomSpeed = 1f;
    
    private float zoomMax = -20;
    private float zoomMin = -2;

    float targetZoom = -10f;
    public float zoomSensitivity = 1.0f;
    public float smoothSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        moveAction.Enable();
        zoomAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Zoom();
    }

    void Move()
    {
        var cameraDirection = moveAction.ReadValue<Vector2>();

        if (cameraDirection == Vector2.zero)
            return;

        transform.position = new Vector3(
            transform.position.x + cameraDirection.x * moveSpeed * Time.deltaTime, 
            transform.position.y + cameraDirection.y * moveSpeed * Time.deltaTime, 
            transform.position.z
        );
    }

    void Zoom()
    {
        float scrollInput = zoomAction.ReadValue<Vector2>().y;

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            targetZoom += scrollInput * zoomSensitivity;
            targetZoom = Mathf.Clamp(targetZoom, zoomMax, zoomMin);
        }

        if (Mathf.Abs(transform.position.z - targetZoom) > 0.0001f)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

            // Save the mouse pos before zooming
            mouseScreenPos.z = Mathf.Abs(transform.position.z);
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(mouseScreenPos);

            // Zooms the camera
            float newZ = Mathf.Lerp(transform.position.z, targetZoom, Time.deltaTime * smoothSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

            // Retrieve mouse pos after zooming
            mouseScreenPos.z = Mathf.Abs(transform.position.z);
            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(mouseScreenPos);

            // Calculate the difference between before and after
            Vector3 difference = mouseWorldBefore - mouseWorldAfter;
            // Move camera based on difference
            transform.position += new Vector3(difference.x, difference.y, 0);
        }
    }
}
