using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifePool : MonoBehaviour
{
    public GameObject seed;
    public GameStateManager manager;
    public int animationSpeed = 5;
    public int scoreMax = 5;
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
        counter = Mathf.Clamp(counter, -scoreMax, scoreMax);
        SendCounterToPosition();
        if (counter >= scoreMax)
        {
            DoWin();
        }

        if (counter <= -scoreMax)
        {
            DoLose();
        }
    }

    private void DoWin()
    {
        Debug.Log("You win!");
    }

    private void DoLose()
    {
        Debug.Log("You lose!");
    }

    private void SendCounterToPosition()
    {
        Vector3 targetPos = new Vector3(offset + (increment * counter), 0, 0);
		StopAllCoroutines();
        StartCoroutine(SeedToPosition(targetPos));
    }
    
    IEnumerator SeedToPosition(Vector3 targetPosition)
    {
        if (seed == null) yield break;

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
