using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckLayout : MonoBehaviour
{
    public List<GameObject> cards;
    public float yIncrement = 0.2f;
    public float animationSpeed = 5f; // Speed of the animation
    private int previousChildCount = 0;
    public GameObject hand;

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
			GameObject cardObject = transform.GetChild(i).gameObject;
            cards.Add(cardObject);
			Card card = cardObject.GetComponent<Card>();
			card.isDraggable = false;
        }
    }

	[ContextMenu("Draw Card")]
    public void DrawCard()
    {
        if (cards.Count == 0) return;
        StopAllCoroutines();
        GameObject card = cards[cards.Count - 1];
        cards.Remove(card);
        previousChildCount--;
        card.transform.SetParent(hand.transform);
		

        // Reset card's animation state
        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent != null)
        {
			cardComponent.isDraggable = true;
            cardComponent.isReturning = false;
        }
    }

    public void ArrangeCards()
    {
        if (cards.Count == 0) return;
        // Calculate the offset to center the cards
        float totalHeight = (cards.Count - 1) * yIncrement;

        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y += i * yIncrement;
            // Animate to the target position
            StartCoroutine(AnimateToPosition(cards[i], targetPosition));
        }
    }

    IEnumerator AnimateToPosition(GameObject card, Vector3 targetPosition)
    {
        if (card == null) yield break;

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            // Clamp lerp factor to prevent instant snapping on frame time spikes
            float lerpFactor = Mathf.Min(Time.deltaTime * animationSpeed, 0.5f);
            card.transform.position = Vector3.Lerp(
                card.transform.position,
                targetPosition,
                lerpFactor
            );
            yield return null;
        }
        
        // Snap to final position
        card.transform.position = targetPosition;
    }
}