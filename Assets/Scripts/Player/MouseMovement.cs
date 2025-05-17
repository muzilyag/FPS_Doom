using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    private float xRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
        else
            transform.Rotate(Vector3.up * mouseX);
    }
}