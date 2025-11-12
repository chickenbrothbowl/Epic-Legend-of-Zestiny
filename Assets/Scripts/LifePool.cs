using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifePool : MonoBehaviour
{
    public GameObject seed;
    public GameStateMonitor monitor;
    public int animationSpeed = 5; 
    public int counter = 0;

    public float offset = -0.025f;
    public float increment = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = 0;
    }

    public void MoveCounter(int amnt)
    {
        counter += amnt;
        counter = Mathf.Clamp(counter, -5, 5);
        SendCounterToPosition();
    }

    private void SendCounterToPosition()
    {
		Debug.Log("Attemting to Animate!");
        Vector3 targetPos = new Vector3(offset + (increment * counter), 0, 0);
        StartCoroutine(SeedToPosition(targetPos));
    }
    
    IEnumerator SeedToPosition(Vector3 targetPosition)
    {
		Debug.Log("Animating!1");
        if (seed == null) yield break;
		Debug.Log("Animating!");

        while (Vector3.Distance(seed.transform.localPosition, targetPosition) > 0.01f)
        {
            // Clamp lerp factor to prevent instant snapping on frame time spikes
            float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
            seed.transform.localPosition = Vector3.Lerp(
                seed.transform.localPosition,
                targetPosition,
                lerpFactor
            );
            yield return null;
        }
        // Snap to final position
        seed.transform.localPosition = targetPosition;
    }
}
