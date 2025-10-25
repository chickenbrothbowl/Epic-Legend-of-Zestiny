using UnityEngine;
using System.Collections;

public class CardHandLayout : MonoBehaviour
{
    public GameObject[] cards;
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
        cards = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cards[i] = transform.GetChild(i).gameObject;
        }
    }


    public void ArrangeCards()
    {
        if (cards.Length == 0) return;

        // Calculate the offset to center the cards
        float totalWidth = (cards.Length - 1) * xSpacing;
        float startOffset = centerCards ? -totalWidth / 2f : 0f;

        for (int i = 0; i < cards.Length; i++)
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

        while (Vector3.Distance(card.transform.position, targetPosition) > 0.01f)
        {
            card.transform.position = Vector3.Lerp(
                card.transform.position, 
                targetPosition, 
                Time.deltaTime * animationSpeed
            );
            yield return null;
        }
        
        // Snap to final position
        card.transform.position = targetPosition;
    }
}