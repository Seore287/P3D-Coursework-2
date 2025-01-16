using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool followPlayerRotation = true;

    [Header("Camera Rotation Settings")]
    [SerializeField] private CameraRotationSettings cameraRotationSettings;

    public Transform Player
    {
        get => player;
        set
        {
            if (value != null)
                player = value;
            else
                Debug.LogWarning("Attempting to set players reference");
        }
    }

    public Vector3 CameraOffset
    {
        get => offset;
        set => offset = value;
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = Mathf.Max(0, value);
    }

    public bool FollowPlayerRotation
    {
        get => followPlayerRotation;
        set => followPlayerRotation = value;
    }

    public CameraRotationSettings RotationSettings => cameraRotationSettings;

    private void LateUpdate()
    {
        //Calculates the camera's position
        Vector3 desiredPosition = player.position + Quaternion.Euler(cameraRotationSettings.VerticalAngle, cameraRotationSettings.HorizontalAngle, 0) * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationSpeed);

        transform.LookAt(player.position);

        //Camera rotates with the player
        if (followPlayerRotation)
        {
            cameraRotationSettings.HorizontalAngle = player.eulerAngles.y;
        }
    }
}
