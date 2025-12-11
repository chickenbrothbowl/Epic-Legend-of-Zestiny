using UnityEngine;

public class CameraMouseTilt : MonoBehaviour
{
    [Header("Camera Angles")]
    [SerializeField] private float normalAngleX = 0f;
    [SerializeField] private float tiltedAngleX = 30f;
    
    [Header("Mouse Detection")]
    [SerializeField] private float screenBottomThreshold = 100f; // pixels from bottom
    [SerializeField] private float screenBottomThresholdUp = 550f;
    
    [Header("Animation")]
    [SerializeField] private float smoothSpeed = 5f;

    private bool isDown = false;
    
    private float targetAngleX;
    private Quaternion targetRotation;

    void Start()
    {
        targetAngleX = normalAngleX;
    }

    void Update()
    {
        // Check mouse position
        if (Input.mousePosition.y < screenBottomThreshold && !isDown)
        {
            targetAngleX = tiltedAngleX;
            isDown = true;
        }
        else if (Input.mousePosition.y < screenBottomThresholdUp &&isDown)
        {
            targetAngleX = tiltedAngleX;
        }
        else
        {
            targetAngleX = normalAngleX;
            isDown = false;
        }
        
        // Smoothly interpolate to target angle
        targetRotation = Quaternion.Euler(targetAngleX, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}