using UnityEngine;
using System.Collections;

public class Bell : MonoBehaviour
{
    [Header("References")]
    public GameStateManager gameStateManager;
    public Transform bellTop;
    
    [Header("Shake Settings")]
    public float shakeSpeed = 8f;
    public float shakeAmount = 10f; // Degrees of rotation
    
    public bool isShaking = false;
    private Quaternion originalRotation;

    void Start()
    {
        if (bellTop != null)
        {
            originalRotation = bellTop.localRotation;
        }
    }

    void Update()
    {
        // Start/stop shaking based on actual juice level
        if (gameStateManager.juice.GetJuice() <= 0 && !isShaking)
        {
            StartShaking();
        }
        else if (gameStateManager.juice.GetJuice() > 0 && isShaking)
        {
            StopShaking();
        }
    }

    void OnMouseDown()
    {
        if (gameStateManager.isPlayerTurn)
        {
            AudioManager.Instance.BellTapsound();
            gameStateManager.EndPlayerTurn();
        }
    }

    void StartShaking()
    {
        if (bellTop != null)
        {
            isShaking = true;
            StartCoroutine(ShakeRoutine());
        }
    }

    void StopShaking()
    {
        isShaking = false;
        StopAllCoroutines();
        if (bellTop != null)
        {
            bellTop.localRotation = originalRotation;
        }
    }

    IEnumerator ShakeRoutine()
    {
        while (isShaking)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            bellTop.localRotation = originalRotation * Quaternion.Euler(shake, shake, shake);
            yield return null;
        }
    }
}