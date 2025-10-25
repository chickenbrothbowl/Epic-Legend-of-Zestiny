using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardHandLayout : MonoBehaviour
{
    public List<GameObject> cards;
    public float xSpacing = 1.5f;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f; // Speed of the animation
    public bool centerCards = true; // Center the hand around the parent position

    private int previousChildCount = 0;

    void Start()
    {
        UpdateCardsArray();
        ArrangeCards();
    }

    void Update()
    {
        // Only update the array if the number of children changed
        if (transform.childCount != previousChildCount)
        {
            UpdateCardsArray();
            ArrangeCards();
            previousChildCount = transform.childCount;
        }
    }

    void UpdateCardsArray()
    {
        cards = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            cards.Add(transform.GetChild(i).gameObject);
        }
    }


    public void ArrangeCards()
    {
        if (cards.Count == 0) return;

        Debug.Log($"[CardHandLayout] ArrangeCards called for {cards.Count} cards");
        StopAllCoroutines();

        // Calculate the offset to center the cards
        float totalWidth = (cards.Count - 1) * xSpacing;
        float startOffset = centerCards ? -totalWidth / 2f : 0f;

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x += startOffset + (i * xSpacing);
            targetPosition.y += i * yIncrement;
			
            
            // Animate to the target position
            StartCoroutine(AnimateToPosition(cards[i], targetPosition));
        }
    }

    IEnumerator AnimateToPosition(GameObject card, Vector3 targetPosition)
    {
        if (card == null) yield break;

        Debug.Log($"[CardHandLayout] Starting animation for {card.name} from {card.transform.position} to {targetPosition}");
        int frameCount = 0;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
			Debug.Log($"[CardHandLayout] Frame {frameCount}: Time.deltaTime={Time.deltaTime}, distance={Vector3.Distance(card.transform.position, targetPosition)}");
            // Clamp lerp factor to prevent instant snapping on frame time spikes
            float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
            card.transform.position = Vector3.Lerp(
                card.transform.position,
                targetPosition,
                lerpFactor
            );
            frameCount++;
            yield return null;
        }

        Debug.Log($"[CardHandLayout] Animation completed for {card.name} in {frameCount} frames");
        // Snap to final position
        card.transform.position = targetPosition;
    }
}