using UnityEngine;
using System.Collections;

public class DeckLayout : MonoBehaviour
{
    public GameObject[] cards;
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
        cards = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cards[i] = transform.GetChild(i).gameObject;
        }
    }

	[ContextMenu("Draw Card")]
    public void DrawCard()
    {
        if (transform.childCount != 0)
        {
            Transform card = transform.GetChild(transform.childCount-1);
            card.SetParent(hand.transform);
        }
        
    }

    public void ArrangeCards()
    {
        if (cards.Length == 0) return;
        // Calculate the offset to center the cards
        float totalHeight = (cards.Length - 1) * yIncrement;
        
        for (int i = 0; i < cards.Length; i++)
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